namespace LibraryApp
{
    public class Library
    {
        private readonly string _name;

        // Minden fizikai példány egy külön string bejegyzés a listában.
        // Pl. 3 példány után: _availableBooks = ["Dune", "Dune", "Dune"]
        // Kölcsönzéskor: egy bejegyzés átkerül _availableBooks -> _borrowedBooks
        // Visszahozáskor: egy bejegyzés visszakerül _borrowedBooks -> _availableBooks
        private readonly List<string> _availableBooks;
        private readonly List<string> _borrowedBooks;

        // name nem lehet null vagy üres
        public Library(string name)
        {
            if (name == null || name == "")
                throw new ArgumentException();

            _name = name;
            _availableBooks = new List<string>();
            _borrowedBooks = new List<string>();
        }

        public string GetName()
        {
            return _name;
        }

        // Minden példány egy külön bejegyzés — AddBook("Dune", 3) -> három "Dune" kerül a listába
        // copies >= 1
        public void AddBook(string title, int copies)
        {
            if (copies <= 0)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < copies; i++)
                _availableBooks.Add(title);
        }

        // Visszatér false-al ha nincs elérhető példány a megadott címből
        public bool BorrowBook(string title)
        {
            for (int i = 0; i < _availableBooks.Count; i++)
            {
                if (_availableBooks[i] == title)
                {
                    _availableBooks.RemoveAt(i);
                    _borrowedBooks.Add(title);
                    return true;
                }
            }
            return false;
        }

        // Visszatér false-al ha nincs kikölcsönzött példány a megadott címből
        public bool ReturnBook(string title)
        {
            for (int i = 0; i < _borrowedBooks.Count; i++)
            {
                if (_borrowedBooks[i] == title)
                {
                    _borrowedBooks.RemoveAt(i);
                    _availableBooks.Add(title);
                    return true;
                }
            }

            return false;
        }

        // Az _availableBooks listában szereplő példányok számát adja vissza — -1 ha a cím nem szerepel
        public int GetAvailableCopies(string title)
        {
            bool exists = false;
            for (int i = 0; i < _availableBooks.Count; i++)
            {
                if (_availableBooks[i] == title)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                for (int i = 0; i < _borrowedBooks.Count; i++)
                {
                    if (_borrowedBooks[i] == title)
                    {
                        exists = true;
                        break;
                    }
                }
            }
            if (!exists)
            {
                return -1;
            }

            int count = 0;
            for (int i = 0; i < _availableBooks.Count; i++)
            {
                if (_availableBooks[i] == title)
                { 
                    count++; 
                }
                    
            }
            return count;
        }

        // Visszatér true-val ha legalább egy szabad példány elérhető
        public bool IsAvailable(string title)
        {
            for (int i = 0; i < _availableBooks.Count; i++)
            {
                if (_availableBooks[i] == title)
                {
                    return true;
                }
                    
            }
            return false;
        }

        // Az összes egyedi cím száma (elérhető és kikölcsönzött együtt)
        public int GetTotalTitles()
        {
            List<string> uniqueTitles = new List<string>();
            for (int i = 0; i < _availableBooks.Count; i++)
            {
                bool alreadyAdded = false;
                for (int j = 0; j < uniqueTitles.Count; j++)
                {
                    if (uniqueTitles[j] == _availableBooks[i])
                    {
                        alreadyAdded = true;
                        break;
                    }
                }
                if (!alreadyAdded)
                {
                    uniqueTitles.Add(_availableBooks[i]);
                }    
            }

            for (int i = 0; i < _borrowedBooks.Count; i++)
            {
                bool alreadyAdded = false;
                for (int j = 0; j < uniqueTitles.Count; j++)
                {
                    if (uniqueTitles[j] == _borrowedBooks[i])
                    {
                        alreadyAdded = true;
                        break;
                    }
                }
                if (!alreadyAdded)
                    uniqueTitles.Add(_borrowedBooks[i]);
            }
            return uniqueTitles.Count;
        }

        // Az összes jelenleg kikölcsönzött példány száma
        public int GetTotalBorrowed()
        {
            return _borrowedBooks.Count;
        }

        // Eltávolít minden példányt — visszatér false ha a cím nem létezik
        public bool RemoveBook(string title)
        {
            bool exists = false;
            for (int i = 0; i < _availableBooks.Count; i++)
            {
                if (_availableBooks[i] == title)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                for (int i = 0; i < _borrowedBooks.Count; i++)
                {
                    if (_borrowedBooks[i] == title)
                    {
                        exists = true;
                        break;
                    }
                }
            }


            if (!exists)
            {
                return false;
            }
            for (int i = _availableBooks.Count - 1; i >= 0; i--)
            {
                if (_availableBooks[i] == title)
                    _availableBooks.RemoveAt(i);
            }

            for (int i = _borrowedBooks.Count - 1; i >= 0; i--)
            {
                if (_borrowedBooks[i] == title)
                    _borrowedBooks.RemoveAt(i);
            }
            return true;
        }
    }
}
