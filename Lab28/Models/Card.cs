using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab28.Models
{
    public class Card
    {
        public string Image { get; set; }
        public string Value { get; set; }
        public string Suit { get; set; }

        public Card()
        {

        }

        public Card(string ima, string val, string suit)
        {
            Image = ima;
            Value = val;
            Suit = suit;
        }

    }
}