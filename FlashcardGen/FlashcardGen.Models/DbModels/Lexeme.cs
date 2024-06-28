﻿namespace FlashcardGen.Models.DbModels
{
    public class Lexeme
    {
        public int LexemeId { get; set; }

        public string ExtendedStrongsNumber { get; set; }
        public string TyndaleHouseGloss { get; set; }
        public string LexicalForm {  get; set; }

        public virtual ICollection<WordForm> WordForms { get; set; }
    }
}