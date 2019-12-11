using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Mathtasic_Voyage {
    class Queue<T> {
        private Node<T> _head;
        private Node<T> _tail;
        private int _size;
        public int Size {
            get {return _size;}
        }
        public bool IsEmpty {
            get {return _size > 0;}
        }
        //constructor
        public Queue() {
            _head = null;
            _tail = null;
            _size = 0;
        }
        public Queue(T newData) {
            _tail = new Node<T>(newData);
            _head = _tail;
            _size = 1;
        }

        //methods
        public void Enqueue(T newData) {
            Node<T> newNode = new Node<T>(newData);
            if (_tail != null) {
                _tail.Previous = newNode;
                newNode.Next = _tail;
                _tail = _tail.Previous;
                _size++;
            } else {
                _tail = newNode;
                _head = _tail;
                _size++;
            }
        }
        public T Dequeue() {
            try {
                T result = _head.Data;
                _head = _head.Previous;
                if (_head != null) {
                    _head.Next = null;
                } else {
                    _tail = null;
                }
                _size--;
                return result;

            }catch(NullReferenceException e){
                Debug.WriteLine(e + "\nQueue is empty.");
                return default;
            }
        }
        public T Peek() {
            try {
                return _head.Data;
            } catch(NullReferenceException e) {
                Debug.WriteLine(e + " \nQueue is empty.");
                throw;
            }
        }
        public void Clear() {
            _head = null;
            _tail = _head;
        }
        public override string ToString() {
            Node<T> current = _head;
            string result = "";
            if (_tail == null) {
                return "Queue is empty";
            }
            for (int i = 0; i < _size; i++) {
                if (i != _size - 1) {
                    result += "["+current.Data.ToString()+"], ";
                } else {
                    result += "["+current.Data.ToString()+"]";
                }
                current = current.Previous;
            }
            return result;
        }
    }
}
