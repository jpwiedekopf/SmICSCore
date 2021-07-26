﻿
using SmICSCoreLib.AQL.PatientInformation.Symptome;
using SmICSDataGenerator.Tests;
using System.Collections.Generic;
using SmICSFactory.Tests;
using System.Collections;
using SmICSCoreLib.REST;
using Xunit;

namespace WebApp.Test.Symptom
{
    public class GetAllSymptomTest
    {
        [Theory]
        [ClassData(typeof(SymptomTestData))]
        public void ProcessorTest(int ResultSetID)
        {
            RestDataAccess _data = TestConnection.Initialize();

            SymptomFactory factory = new SymptomFactory(_data);
            List<SymptomModel> actual = factory.ProcessNoParam();
            List<SymptomModel> expected = GetSymptom(ResultSetID);

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < 9; i++)
            {
                Assert.Equal(expected[i].NameDesSymptoms, actual[i].NameDesSymptoms);
                Assert.Equal(expected[i].Anzahl_Patienten, actual[i].Anzahl_Patienten);
            }

        }

        private class SymptomTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                 yield return new object[] { 0 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private List<SymptomModel> GetSymptom(int ResultSetID)
        {
            string testResultPath = "../../../../WebApp.Test/Resources/AllSymptomResult.json";
            string parameterPath = "../../../../WebApp.Test/Resources/Empty_Symptome.json";

            List<SymptomModel> result = ExpectedResultJsonReader.ReadResults<SymptomModel, PatientInfos>(testResultPath, parameterPath, ResultSetID, ExpectedType.SYMPTOM_MODEL);
            return result;
        }
    }
}
