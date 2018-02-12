using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {

    // Generic Deck class for "card" shuffling and drawing
    public class Deck<T> {
        List<T> def;
        List<T> deck;

        public int Size {
            get { return def.Count; }
        }

        public int Left {
            get { return deck.Count; }
        }

        public Deck() {
            def = new List<T>();
            deck = new List<T>();
            Shuffle();
        }

        public Deck(List<T> list) {
            foreach (T t in list) {
                def.Add(t);
                deck.Add(t);
            }
            Shuffle();
        }

        public void Add(T elmt, int num = 1) {
            for (int i = 0; i < num; ++i) {
                def.Add(elmt);
                deck.Add(elmt);
            }
        }

        // Fisher-Yates uniform shuffle
        public void Shuffle() {
            int rIndex;
            T temp;
            for (int i = deck.Count - 1; i >= 1; --i) {
                rIndex = Random.Range(0, i + 1);
                temp = deck[rIndex];
                deck[rIndex] = deck[i];
                deck[i] = temp;
            }
        }

        public void Reset() {
            deck.Clear();

            foreach (T t in def) {
                deck.Add(t);
            }

            Shuffle();
        }

        public T Draw() {
            int ind = deck.Count - 1;
            T next = deck[ind];
            deck.RemoveAt(ind);

            if (deck.Count == 0) {
                Reset();
            }

            return next;
        }
    }
}
