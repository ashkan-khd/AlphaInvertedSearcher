﻿using System;
using System.Collections.Generic;
using System.Reflection;
using AlphaInvertedSearcher.Engine;
using AlphaInvertedSearcher.Engine.Query;
using AlphaInvertedSearcher.InvertedMap;
using AlphaInvertedSearcherTest.InvertedMapTest;
using Xunit;

namespace AlphaInvertedSearcherTest.EngineTest
{
    public class AlphaEngineTest
    {
        private Map _map = MapTest.InitMapper();
        private AlphaEngine _alphaEngine;

        [Fact]
        public void AddKeyword()
        {
            _alphaEngine = new AlphaEngine(_map);
            AlphaEngine alphaEngine = _alphaEngine
                .AddMustIncludes("hello", "hi", "hMM")
                .AddExcludes("he", "She", "mEn", "WOmen")
                .AddMustIncludes("AhA")
                .AddLeastIncludes("Ok", "There")
                .AddExcludes("How", "wheRe", "some");
            SearchQuery query = (SearchQuery)GetInstanceField(typeof(AlphaEngine), alphaEngine, "_query");
            Assert.Equal(new HashSet<string>(){"hello", "hi", "hmm", "aha"}, query.norms);
            Assert.Equal(new HashSet<string>(){"ok", "there"}, query.poss);
            Assert.Equal(new HashSet<string>(){"he", "she", "men", "women", "how", "where", "some"}, query.negs);
        }
        
        static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }

        [Fact]
        public void ExecuteQueryTest()
        {
            _alphaEngine = new AlphaEngine(_map);
            AlphaEngine engine = _alphaEngine.AddMustIncludes("The", "WitcheR", "A").AddLeastIncludes("HunTeR", "Bank");
            AlphaEngine copyEngine = engine.Clone();
            Assert.Equal(new List<string>(){"1.txt", "2.txt", "3.txt"}, engine.ExecuteQuery());
            Assert.Equal(new List<string>(){"2.txt"}, copyEngine.AddExcludes("role-playing").ExecuteQuery());
        }
        
    }
}