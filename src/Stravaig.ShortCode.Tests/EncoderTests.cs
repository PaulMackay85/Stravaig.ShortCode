﻿using System;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.ShortCode.Tests
{
    [TestFixture]
    public class EncoderTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Ctor_WithNullOrWhitespace(string value)
        {
            ArgumentException ex = Should.Throw<ArgumentException>(() => new Encoder(value));
            ex.ParamName.ShouldBe("characterSpace");
            ex.Message.ShouldStartWith("Value cannot be null or whitespace.");
        }

        [Test]
        public void Ctor_WithSingleCharacter()
        {
            ArgumentException ex = Should.Throw<ArgumentException>(() => new Encoder("1"));
            ex.ParamName.ShouldBe("characterSpace");
            ex.Message.ShouldStartWith("Value cannot have a single character.");
        }
        
        [TestCase(1, Encoder.Digits, ExpectedResult = (ulong)10)]
        [TestCase(2, Encoder.Digits, ExpectedResult = (ulong)100)]
        [TestCase(1, Encoder.LowerLatinLetters, ExpectedResult = (ulong)26)]
        [TestCase(2, Encoder.LowerLatinLetters, ExpectedResult = (ulong)676)]
        [TestCase(3, Encoder.LatinLetters, ExpectedResult = (ulong)140608)]
        [TestCase(4, Encoder.LettersAndDigits, ExpectedResult = (ulong)14776336)]
        [TestCase(5, Encoder.LettersAndDigits, ExpectedResult = (ulong)916132832)]
        public ulong RangeRequiredReturnsGoodValueForCharacterSpace(int numChars, string charSpace)
        {
            var characterSpace = new Encoder(charSpace);
            return characterSpace.RangeRequired(numChars);
        }

        [TestCase(Encoder.LettersAndDigits, ExpectedResult = 10)]
        [TestCase(Encoder.Digits, ExpectedResult = 19)]
        public int MaxLengthTests(string charSpace)
        {
            var characterSpace = new Encoder(charSpace);
            return characterSpace.MaxLength();
        }

        [TestCase((ulong)1, Encoder.Digits, ExpectedResult = "1")]
        [TestCase((ulong)10, Encoder.Digits, ExpectedResult = "10")]
        [TestCase((ulong)1, Encoder.UpperLatinLetters, ExpectedResult = "B")]
        [TestCase((ulong)10, Encoder.UpperLatinLetters, ExpectedResult = "K")]
        [TestCase((ulong)25, Encoder.UpperLatinLetters, ExpectedResult = "Z")]
        [TestCase((ulong)26, Encoder.UpperLatinLetters, ExpectedResult = "BA")]
        [TestCase((ulong)27, Encoder.UpperLatinLetters, ExpectedResult = "BB")]
        public string ConvertTests(ulong code, string charSpace)
        {
            var characterSpace = new Encoder(charSpace);
            return characterSpace.Convert(code);
        }
        
        [TestCase((ulong)1, Encoder.Digits, ExpectedResult = "001")]
        [TestCase((ulong)10, Encoder.Digits, ExpectedResult = "010")]
        [TestCase((ulong)1, Encoder.UpperLatinLetters, ExpectedResult = "AAB")]
        [TestCase((ulong)10, Encoder.UpperLatinLetters, ExpectedResult = "AAK")]
        [TestCase((ulong)25, Encoder.UpperLatinLetters, ExpectedResult = "AAZ")]
        [TestCase((ulong)26, Encoder.UpperLatinLetters, ExpectedResult = "ABA")]
        [TestCase((ulong)27, Encoder.UpperLatinLetters, ExpectedResult = "ABB")]
        public string ConvertWithFixedWidthTests(ulong code, string charSpace)
        {
            var characterSpace = new Encoder(charSpace);
            return characterSpace.Convert(code, 3);
        }

        [Test]
        public void Convert_WithZeroInput_ThrowsException()
        {
            var cs = new Encoder(Encoder.Digits);

            Should.Throw<ArgumentException>(() => cs.Convert(0));
        }
    }
}