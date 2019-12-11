using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Project_Mathtasic_Voyage {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        #region GLOBALS
        Queue<MathProblem> questionQueue = new Queue<MathProblem>();
        #endregion
        #region BUTTON EVENTS
        private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
            CalculateAnswer();
        }

        private void BtnQueue_Click(object sender, RoutedEventArgs e) {
            QueueProblems();
        }
        #endregion
        #region GET/GIVE DATA
        string[] ReadFile() {
            OpenFileDialog myFile = new OpenFileDialog {
                Filter = "Text files (*.txt)|*.txt|Docx files (*.docx)|*.docx;|All files (*.*)|*.*"
            };

            if (myFile.ShowDialog() == true) {
                return File.ReadAllLines(myFile.FileName); 

            }

            return null;
        }
        void QueueProblems() {
            //reading file info
            
            string[] problems = ReadFile();

            if (problems != null) {
                foreach (string problem in problems) {
                    if (IsMathProblem(RemoveExcessSpace(problem))) {
                        questionQueue.Enqueue(new MathProblem(problem));
                        LsbMathProblems.Items.Add(RemoveExcessSpace(problem));
                    }
                }
                ShowCurrentProblem();
            }//end if
        }
        #endregion
        #region TEXTBOX MANIPULATORS
        private void TxtInputAnswer_TextChanged(object sender, TextChangedEventArgs e) {
            TxtInputAnswer.Background = Brushes.White;
        }
        #endregion
        #region BTNSUBMIT ONCLICK
        private void ShowCurrentProblem() {
            if (LsbMathProblems.Items.Count != 0) {
                //convert problem at top of the queue to string and print to txt box
                TxtDisplayProblem.Text = questionQueue.Peek().Problem.ToString() + " =";
            }
        }
        private void CalculateAnswer() {
            if (questionQueue.Size > 0) {
                //gets user input from txt box and calculates whether the answer is correct, then sorts to appropriate box
                SortToList();
                //if there are no more questions, post final score, clear/hide all inputs and lock button
                if(questionQueue.Size == 0) {
                    HideAll();
                }
            }
            
            
        }
        private void SortToList() {
            bool isNum = double.TryParse(TxtInputAnswer.Text,out double num);
            if (isNum) {
                Solving();
                //assign user input to the "useranswer" at top of queue
                questionQueue.Peek().UserAnswer = num;
                //put question and user answer into string form
                string answer = questionQueue.Peek().Problem.ToString() + " = " + num.ToString();

                if (questionQueue.Peek().Correct) {
                    //add to correct box
                    LsbCorrectProblems.Items.Add(answer);
                } else {
                    //add to incorrect box
                    LsbIncorrectProblems.Items.Add(answer);
                }

                //remove answered question from queue
                questionQueue.Dequeue();
                //remove answered question from list
                LsbMathProblems.Items.RemoveAt(0);
                //update current problem
                ShowCurrentProblem();
                //clear user input 
                TxtInputAnswer.Clear();
                //clear error box
                TxtErrorBox.Clear();


            } else {
                TxtInputAnswer.Background = Brushes.IndianRed;
                TxtErrorBox.Text = "ERROR: incompatible symbol in answer box...";
            }
        }
        private string CalculateScore() {
            //store values to doubles to make sure division works properly
            double    totalQuestions = LsbCorrectProblems.Items.Count + LsbIncorrectProblems.Items.Count;
            double    correctAnswers = LsbCorrectProblems.Items.Count;
            double    score = (correctAnswers/totalQuestions)*100;        
            //cast score to int to display proper score
            return $"{(int)score}%";
        }
        #endregion
        #region STRING MANIPULATION
        string RemoveExcessSpace(string word) {
            char lastLetter = '\0';
            string result = "";
            foreach (char letter in word) {
                if (letter == ' ' && lastLetter == ' ') {
                    lastLetter = letter;
                    continue;
                }
                result += letter;
                lastLetter = letter;
            }
            return result;
        }
        bool IsMathProblem(string problem) {
            string compatibles = " 0123456789.()+-/%*";
            if(problem.Length < 3) {
                return false;
            }
            foreach (char item in problem) {
                if (compatibles.Contains(item)) {
                    continue;
                } else {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region CALCULATING CORRECT ANSWER
        bool IsOperator(char symbol) {
            string operators = "+-*/%";
            if (operators.Contains(symbol) && symbol != '(' && symbol != ')') return true;
            return false;
        }
        bool IsOperator(string symbol) {
            string operators = "+-*/%";
            if (operators.Contains(symbol) && symbol != "(" && symbol != ")") return true;
            return false;
        }
        int Precedence(string symbol) {
            if (symbol == "*" || symbol == "/" || symbol == "%") return 2;
            else if (symbol == "+" || symbol == "-") return 1;
            else return 0;
        }        
        #region DOMATH
        string ConvertToPostfix() {
            Stack<string> myStack = new Stack<string>();
            string postfix = "";
            //get problem from queue and remove excess space
            questionQueue.Peek().Problem = RemoveExcessSpace(questionQueue.Peek().Problem);
            //save updated problem to string for editing
            string currentProblem = questionQueue.Peek().Problem;
            currentProblem += ")";
            myStack.Push("(");
            //scan infix
            foreach (char item in currentProblem) {
                bool _operator = IsOperator(item);

                if (item == '(') {
                    myStack.Push(item.ToString());
                
                }else if (item == ')') {
                    do {
                        //add space to prevent sandwiching operands and operators
                        postfix += " " + myStack.Pop();
                    }while(myStack.Peek() != "(");
                    myStack.Pop();
                }else if (!_operator || item == ' '){
                    postfix += item;
                }else if (_operator) {
                    //repeatedly pop from stack and add each operator (on the stack) 
                    //which has the same precedence as, or higher than operator to "postfix"
                    string x = myStack.Pop();
                    while(IsOperator(x) && Precedence(x) >= Precedence(item.ToString())) {
                        postfix += " " +x;
                        x = myStack.Pop();
                    }
                    myStack.Push(x);
                    myStack.Push(item.ToString());                
                } else {
                    //bad symbol
                    return "error";
                }
                
            }            
            return postfix;

        }
        string[] PostfixToArray(string postfix) {
            return RemoveExcessSpace(postfix).Split(' ');
        }
        bool Postfix(string[] text) {
            Stack<double> doubleStack = new Stack<double>(); 
            string[] myOperators = {"+","-","/","%","*","x"};

            for (int i = 0; i < text.Length; i++) {
                bool isNumeric = double.TryParse(text[i],out double num);
                if (isNumeric) {
                    //push converted number to stack
                    doubleStack.Push(num);
                
                //check if string is an operator
                } else if (ContainsInStringArray(myOperators,text[i],out int operatorIndex)) {
                    DoMath(myOperators[operatorIndex],doubleStack);
                } else {
                    //incorrect symbol, disallow output
                    return false;
                }
            }//end for

            if (doubleStack.Size == 1) {
                questionQueue.Peek().CorrectAnswer = doubleStack.Pop();
                return true;
            }
            return false;
        }        
        bool ContainsInStringArray(string[] array, string wordToCheck, out int operatorIndex) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == wordToCheck){operatorIndex = i; return true;}
            }
            operatorIndex = -1;
            return false;
        }
        void DoMath(string _operator, Stack<double> myStack) {
            try {
                double num1 = myStack.Pop();
                double num2 = myStack.Pop();
                switch (_operator) {
                    case "+":
                        myStack.Push(num2 + num1);
                        break;
                    case "-":
                        myStack.Push(num2 - num1);
                        break;
                    case "/":
                        myStack.Push(num2 / num1);
                        break;
                    case "%":
                        myStack.Push(num2 % num1);
                        break;
                    case "*":                               
                        myStack.Push(num2 * num1);
                        break;
                    case "x":
                        myStack.Push(num2*num1);
                        break;
                    default:
                        break;
                }

            } catch(NullReferenceException message) {
                Debug.WriteLine(message);
            }catch(Exception message) {
                Debug.WriteLine(message);
            }
        }
        bool Solving() {
            string postfix = ConvertToPostfix();
            if (postfix == "error") {
                return false;
            }
            return Postfix(PostfixToArray(postfix));
        }
        #endregion

        #endregion
        #region HIDE/BLOCK ELEMENTS
        private void HideAll() {
            TxtDisplayProblem.Clear();
            TxtFinalScore.Text         = CalculateScore();
            TxtInputAnswer.BorderBrush = Brushes.White;
            LblAswer.Visibility        = Visibility.Hidden;
            TxtInputAnswer.IsEnabled   = false;
            BtnSubmit.IsEnabled        = false;
            BtnQueue.IsEnabled         = false;
        }
        #endregion

    }
}