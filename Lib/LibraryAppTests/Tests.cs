using LibraryApp;

namespace LibraryAppTests
{
    [TestClass]
    public class Tests
    {
        private Library CreateDefaultLibrary()
        {
            var lib = new Library("City Library");
            lib.AddBook("Dune", 3);
            lib.AddBook("1984", 1);
            return lib;
        }

        // ---- Constructor ----

        [TestMethod]
        public void Constructor_ValidName()
        {
            var lib = new Library("City Library");
            Assert.AreEqual("City Library", lib.GetName());
        }
        // TODO: null vagy üres névvel létrehozva ArgumentException-t kell dobni

        [TestMethod]
        public void Constructor_InvalidName()
        {
            Assert.ThrowsException<ArgumentException>(() => new Library(null));
        }

        // ---- AddBook ----

        [TestMethod]
        public void AddBook_NewTitle()
        {
            var lib = new Library("City Library");
            lib.AddBook("Dune", 2);
            Assert.AreEqual(1, lib.GetTotalTitles());
        }
        // TODO: ugyanazt a címet hozzáadva újabb bejegyzések kerülnek az _availableBooks listába, és GetTotalTitles nem változik
        [TestMethod]
        public void AddBook_ExistingTitle()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            lib.AddBook("Dune", 2);
            Assert.AreEqual(1, lib.GetTotalTitles());
            Assert.AreEqual(5, lib.GetAvailableCopies("Dune"));
        }
        // TODO: copies értéke 0 vagy negatív esetén ArgumentException-t kell dobni
        [TestMethod]
        public void AddBook_InvalidCopies()
        {
            var lib = new Library("City Library");
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", 0));
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", -1));
        }

        // ---- BorrowBook ----

        [TestMethod]
        public void BorrowBook_AvailableCopy()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            bool result = lib.BorrowBook("Dune");
            Assert.IsTrue(result);
            Assert.AreEqual(2, lib.GetAvailableCopies("Dune"));
        }
        // TODO: nem létező cím esetén false-t kell visszaadni és nem dob kivételt
        [TestMethod]
        public void BorrowBook_NonExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.BorrowBook("NonExistingTitle");
            Assert.IsFalse(result);
        }
        // TODO: az összes példány kikölcsönzése után újabb kölcsönzés false-t ad vissza
        [TestMethod]
        public void BorrowBook_ExistingTitle()
        {
            var lib = CreateDefaultLibrary(); // 1984: 1 példány
            bool firstBorrow = lib.BorrowBook("1984");
            bool secondBorrow = lib.BorrowBook("1984");
            Assert.IsTrue(firstBorrow);
            Assert.IsFalse(secondBorrow);
        }

        // ---- ReturnBook ----

        [TestMethod]
        public void ReturnBook_BorrowedCopy()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("1984");
            bool result = lib.ReturnBook("1984");
            Assert.IsTrue(result);
            Assert.AreEqual(1, lib.GetAvailableCopies("1984"));
        }
        // TODO: nem létező cím visszahozásakor false-t kell visszaadni
        [TestMethod]
        public void ReturnBook_NonExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.ReturnBook("NonExistingTitle");
            Assert.IsFalse(result);
        }
        // TODO: olyan könyv visszahozásakor, amelyből semmi sincs kikölcsönzve, false-t kell adni
        [TestMethod]
        public void ReturnBook_NotBorrowedCopy()
        {
            var lib = CreateDefaultLibrary(); // 1984: 1 példány
            bool result = lib.ReturnBook("1984");
            Assert.IsFalse(result);
        }

        // ---- GetAvailableCopies ----

        [TestMethod]
        public void GetAvailableCopies_AfterBorrow()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            lib.BorrowBook("Dune");
            lib.BorrowBook("Dune");
            Assert.AreEqual(1, lib.GetAvailableCopies("Dune"));
        }
        // TODO: nem létező cím esetén -1-et kell visszaadni
        [TestMethod]
        public void GetAvailableCopies_NonExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            int result = lib.GetAvailableCopies("NonExistingTitle");
            Assert.AreEqual(-1, result);
        }

        // ---- IsAvailable ----

        [TestMethod]
        public void IsAvailable_BookWithFreeCopies()
        {
            var lib = CreateDefaultLibrary();
            Assert.IsTrue(lib.IsAvailable("Dune"));
        }
        // TODO: teljesen kikölcsönzött könyv esetén false-t kell visszaadni
        [TestMethod]
        public void IsAvailable_BookWithoutFreeCopies()
        {
            var lib = CreateDefaultLibrary(); // 1984: 1 példány
            lib.BorrowBook("1984");
            Assert.IsFalse(lib.IsAvailable("1984"));
        }
        // TODO: nem létező cím esetén false-t kell visszaadni
        [TestMethod]
        public void IsAvailable_NonExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            Assert.IsFalse(lib.IsAvailable("NonExistingTitle"));
        }

        // ---- GetTotalBorrowed ----

        [TestMethod]
        public void GetTotalBorrowed_AfterMultipleBorrows()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("Dune");
            lib.BorrowBook("1984");
            Assert.AreEqual(2, lib.GetTotalBorrowed());
        }
        // TODO: újonnan létrehozott, üres könyvtárban GetTotalBorrowed() nullát ad vissza
        [TestMethod]
        public void GetTotalBorrowed_EmptyLibrary()
        {
            var lib = new Library("Empty Library");
            Assert.AreEqual(0, lib.GetTotalBorrowed());
        }
        // TODO: visszahozás után a kikölcsönzött darabszám helyesen csökken
        [TestMethod]
        public void GetTotalBorrowed_AfterReturn()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("Dune");
            lib.BorrowBook("1984");
            lib.ReturnBook("Dune");
            Assert.AreEqual(1, lib.GetTotalBorrowed());
        }

        // ---- RemoveBook ----

        [TestMethod]
        public void RemoveBook_ExistingTitle()
        {
            var lib = CreateDefaultLibrary(); // 2 cím
            bool result = lib.RemoveBook("1984");
            Assert.IsTrue(result);
            Assert.AreEqual(1, lib.GetTotalTitles());
        }
        // TODO: nem létező cím eltávolításakor false-t kell visszaadni
        [TestMethod]
        public void RemoveBook_NonExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.RemoveBook("NonExistingTitle");
            Assert.IsFalse(result);
        }
        // TODO: eltávolítás után a cím már nem érhető el, GetAvailableCopies -1-et ad vissza
        [TestMethod]
        public void RemoveBook_AfterRemoval()
        {
            var lib = CreateDefaultLibrary(); // 1984: 1 példány
            lib.RemoveBook("1984");
            int result = lib.GetAvailableCopies("1984");
            Assert.AreEqual(-1, result);
        }
    }
}
