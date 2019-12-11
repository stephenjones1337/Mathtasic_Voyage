using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Mathtasic_Voyage {
    class Stack<T> {
        private Node<T> _head;
        private int _size;

        public Node<T> Head {
            get{return _head;}
        }
        public int Size {
            get {return _size;}
        }
        public bool IsEmpty {
            get {return _size > 0;}
        }
        //constructor
        public Stack() {
            _head = null;
            _size = 0;
        }
        public Stack(T newData) {
            _head = new Node<T>(newData);
            _size = 1;
        }

        //methods
        public void Push(T newData) {
            Node<T> newNode = new Node<T>(newData);
            if (_head != null) {
                _head.Next = newNode;
                newNode.Previous = _head;
                _head = _head.Next;
                _size++;
            } else {
                _head = newNode;
                _size++;
            }
        }
        public T Pop() {
            try {
                T result = _head.Data;
                _head = _head.Previous;
                if (_head != null)
                    _head.Next = null;
                _size--;
                return result;

            }catch(NullReferenceException e){
                Debug.WriteLine(e + "\nStack is empty.");
                return default;
            }
        }
        public T Peek() {
            try {
                return _head.Data;
            } catch(NullReferenceException e) {
                //exception thrown here... 
                Debug.WriteLine(e + " \nStack is empty.");
                throw;
            }
        }
        public override string ToString() {
            Node<T> current = _head;
            string result = "";
            if (_head == null) {
                return "Stack is empty";
            }
            for (int i = 0; i < _size; i++) {
                if (i == _size - 1) {
                    result += "["+current.Data.ToString()+"]";
                } else {
                    result += "["+current.Data.ToString()+"], ";
                }
                current = current.Previous;
            }
            return result;
        }
        
    }
}
