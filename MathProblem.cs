using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Mathtasic_Voyage {
    class MathProblem {
        private string _problemInfix;
        private double _correctAnswer;
        private double _userAsnwer;
        public string Problem {
            get {return _problemInfix;}
            set {_problemInfix = value;}
        }
        public double CorrectAnswer {
            get {return _correctAnswer;}
            set { _correctAnswer = value;}
        }
        public double UserAnswer {
            get {return _userAsnwer;}
            set {_userAsnwer = value;}
        }
        public bool Correct {
            get {return _userAsnwer == _correctAnswer;}
        }
        public MathProblem(string newProblem) {
            _problemInfix = newProblem;
        }
    }
}
