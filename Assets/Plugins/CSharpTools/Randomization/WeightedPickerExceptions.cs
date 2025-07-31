using System;

namespace CSharpTools.Randomization
{
    public class EmptyPickerException : Exception
    {
        public EmptyPickerException() : this("No Elements have been added to the Weighted Picker.")  { }
        public EmptyPickerException(string message) : base(message)  { }

        public EmptyPickerException(string message, Exception inner) : base(message, inner) { }
    }

    public class DuplicatePickerElementException : Exception
    {
        public DuplicatePickerElementException()  { }

        public DuplicatePickerElementException(string message) : base(message)  { }

        public DuplicatePickerElementException(string message, Exception inner) : base(message, inner) { }
    }

    public class MissingPickerElementException : Exception
    {
        public MissingPickerElementException()  { }

        public MissingPickerElementException(string message) : base(message)  { }

        public MissingPickerElementException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidProbabilityException : Exception
    {
        public InvalidProbabilityException()  { }
        public InvalidProbabilityException(string message) : base(message)  { }

        public InvalidProbabilityException(string message, Exception inner) : base(message, inner) { }
    }
}