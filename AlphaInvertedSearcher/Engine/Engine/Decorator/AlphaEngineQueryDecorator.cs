﻿using System.Collections.Generic;
using AlphaInvertedSearcher.InvertedMap;

namespace AlphaInvertedSearcher.Engine
{
    public class AlphaEngineQueryDecorator : AlphaEngine
    {
        private int board;

        public AlphaEngineQueryDecorator(AlphaEngine decorate, int board) : base(decorate)
        {
            this.board = board;
        }
        
        protected override AlphaEngine CreateThisEngine(AlphaEngine decorate) => 
            new AlphaEngineQueryDecorator(decorate, board);

        public override string GetDocByID(string docID) => _decorate.GetDocByID(docID);

        public override List<string> ExecuteQuery()
        {
            var executeQuery = _decorate.ExecuteQuery();
            
            //Decorating ExecuteQuery
            executeQuery = PrintModifiedLines(executeQuery);
            return executeQuery;
        }
        
        public List<string> PrintModifiedLines(List<string> lines)
        {
            List<string> answer = new List<string>();
            if(lines.Count == 0) {
                answer.Add("No Result Found");
                return answer;
            }
            answer.Add(lines.Count + " Case(s) Found As Follows:");
            if(lines.Count <= board + 1) {
                for (int i = 0; i < lines.Count; i++) {
                    answer.Add("\tDoc" + (i + 1) + ": " + lines[i]);
                }
            } else {
                for (int i = 0; i < board; i++) {
                    answer.Add("\tDoc" + (i + 1) + ": " + lines[i]);
                }
                answer.Add("\t\t.\n\t\t.\n\t\t.");
                answer.Add("\tDoc" + lines.Count + ": " + lines[^1]);
            }
            return answer;
        }
        
    }
}