using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Mathtasic_Voyage {
    class Node<T> {
        private T _data;
        private Node<T>   _next;        
        private Node<T>   _previous;

        public T Data {
            get{return _data;}
            set{_data = value;}
        }
        public Node<T> Next {
            get{return _next;}
            set{_next = value;}
        }
        public Node<T> Previous {
            get{return _previous;}
            set{_previous = value;}
        }

        public Node() {
            _data     = default;
            _next     = null;
            _previous = null;
        }
        public Node(T newData) {
            _data     = newData;
            _next     = null;
            _previous = null;
        }
    }
}
