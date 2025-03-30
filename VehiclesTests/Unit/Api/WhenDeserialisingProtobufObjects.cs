using NUnit.Framework.Legacy;
using ProtoBuf;
using Vehicles.Infrastructure.Serialisation;

namespace VehiclesTests.Unit.Api
{
    [Parallelizable]
    public class WhenDeserialisingProtobufObjects
    {
        [ProtoContract]
        private class DataTypesClass
        {
            [ProtoMember(1)]
            public int IntegerValue { get; set; }

            [ProtoMember(2)]
            public long LongValue { get; set; }

            [ProtoMember(3)]
            public float FloatValue { get; set; }

            [ProtoMember(4)]
            public double DoubleValue { get; set; }

            [ProtoMember(5)]
            public bool BooleanValue { get; set; }

            [ProtoMember(6)]
            public string StringValue { get; set; } = string.Empty;

            [ProtoMember(7)]
            public byte[] ByteArrayValue { get; set; } = [];

            [ProtoMember(8)]
            public DateTime DateTimeValue { get; set; }

            [ProtoMember(9)]
            public List<int> IntegerList { get; set; } = [];

            [ProtoMember(10)]
            public Dictionary<string, string> DictionaryItem { get; set; } = [];

            [ProtoMember(11)]
            public DataTypesSubClass SubClassInstance { get; set; } = new();

            [ProtoMember(12)]
            public ExampleEnum EnumValue { get; set; }

            [ProtoMember(13)]
            public Guid GuidValue { get; set; }
        }

        [ProtoContract]
        private class DataTypesSubClass
        {
            [ProtoMember(1)]
            public string SubClassPropertyOne { get; set; } = string.Empty;

            [ProtoMember(2)]
            public int SubClassPropertyTwo { get; set; }
        }

        [ProtoContract]
        private enum ExampleEnum
        {
            [ProtoEnum] NoThing = 0,
            [ProtoEnum] ThingOne = 1,
            [ProtoEnum] ThingTwo = 2
        }

        private DataTypesClass? _input;
        private DataTypesClass? _output;

        [OneTimeSetUp]
        public void Setup()
        {
            _input = new()
            {
                IntegerValue = 42,
                LongValue = 12345678901234L,
                FloatValue = 3.14f,
                DoubleValue = 2.718,
                BooleanValue = true,
                StringValue = "Stringy the string",
                ByteArrayValue = [1, 2, 3, 4],
                DateTimeValue = new DateTime(2025, 2, 1, 10, 11, 0),
                IntegerList = [1, 2, 3],
                DictionaryItem = new Dictionary<string, string> { { "Homer", "Simpson" } },
                SubClassInstance = new DataTypesSubClass { SubClassPropertyOne = "Liam", SubClassPropertyTwo = 007 },
                EnumValue = ExampleEnum.ThingTwo,
                GuidValue = Guid.NewGuid()
            };

            byte[] serialized = ProtobufHelper.SerialiseToProtobuf(_input);
            _output = ProtobufHelper.DeserialiseFromProtobuf<DataTypesClass>(serialized);
        }

        [Test]
        public void TheReturnedIntegerMatchesTheInputInteger() => Assert.That(_input!.IntegerValue, Is.EqualTo(_output!.IntegerValue));

        [Test]
        public void TheReturnedLongMatchesTheInputLong() => Assert.That(_input!.LongValue, Is.EqualTo(_output!.LongValue));

        [Test]
        public void TheReturnedFloatMatchesTheInputFloat() => Assert.That(_input!.FloatValue, Is.EqualTo(_output!.FloatValue));

        [Test]
        public void TheReturnedDoubleMatchesTheInputDouble() => Assert.That(_input!.DoubleValue, Is.EqualTo(_output!.DoubleValue));

        [Test]
        public void TheReturnedBooleanMatchesTheInputBoolean() => Assert.That(_input!.BooleanValue, Is.EqualTo(_output!.BooleanValue));

        [Test]
        public void TheReturnedStringMatchesTheInputString() => Assert.That(_input!.StringValue, Is.EqualTo(_output!.StringValue));

        [Test]
        public void TheReturnedByteArrayMatchesTheInputByteArray() => CollectionAssert.AreEqual(_input!.ByteArrayValue, _output!.ByteArrayValue);

        [Test]
        public void TheReturnedDateTimeMatchesTheInputDateTime() => Assert.That(_input!.DateTimeValue, Is.EqualTo(_output!.DateTimeValue));

        [Test]
        public void TheReturnedIntegerListMatchesTheInputIntegerList() => CollectionAssert.AreEqual(_input!.IntegerList, _output!.IntegerList);

        [Test]
        public void TheReturnedDictionaryMatchesTheInputDictionary() => CollectionAssert.AreEqual(_input!.DictionaryItem, _output!.DictionaryItem);

        [Test]
        public void TheReturnedSubClassPropertyOneMatchesTheInputSubClassPropertyOne() => Assert.That(_input!.SubClassInstance.SubClassPropertyOne, Is.EqualTo(_output!.SubClassInstance.SubClassPropertyOne));

        [Test]
        public void TheReturnedSubClassPropertyTwoMatchesTheInputSubClassPropertyTwo() => Assert.That(_input!.SubClassInstance.SubClassPropertyTwo, Is.EqualTo(_output!.SubClassInstance.SubClassPropertyTwo));

        [Test]
        public void TheReturnedEnumMatchesTheInputEnum() => Assert.That(_input!.EnumValue, Is.EqualTo(_output!.EnumValue));

        [Test]
        public void TheReturnedGuidMatchesTheInputGuid() => Assert.That(_input!.GuidValue, Is.EqualTo(_output!.GuidValue));

    }
}
