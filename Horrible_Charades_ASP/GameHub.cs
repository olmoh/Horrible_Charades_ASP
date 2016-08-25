﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Horrible_Charades_ASP.Database;
using Horrible_Charades_ASP.Models;

namespace Horrible_Charades_ASP
{
    public class GameHub : Hub
    {
        DatabaseUtils _dbUtils = new DatabaseUtils(
            new Models.CharadeContext());
        Charade _charade = new Charade();

        /// <summary>
        /// This function calls the method Hello on Client-Side, with something to write out
        /// </summary>
        /// <param name="textToWrite"></param>
        public void Hello(string textToWrite)
        {
            Clients.All.hello(textToWrite);
        }
        public void CreateGame() // När man trycker på New game ska man komma hit
        {
            Game game = GameState.Instance.CreateGame();
            Clients.Caller.printGameCode(game); //Todo: skapa printGameCode på klientsidan
        }
        /// <summary>
        /// This function calls the method InsertCharadeHTML on Client-Side, which inserts provided string in a <div>-tag
        /// </summary>
        /// <param name="htmlToWrite"></param>
        public void InsertCharadeHTML(string htmlToWrite)
        {
            Clients.All.insertCharadeHTML(htmlToWrite);
        }

        /// <summary>
        /// Creates a new team if the device don't have a team. 
        /// </summary>
        /// <param name="teamName"></param>
        public void CreateTeam(string teamName, string gameCode) //To-do: validera team-name
        {
            var team = GameState.Instance.GetTeam(teamName);
            if (team != null)
            {
                Clients.All.teamsJoined(team.Name, team.ConnectionID);
            }
            else
            {
                Game game = GameState.Instance.CreateTeam(teamName, gameCode, Context.ConnectionId);
                Clients.All.teamsJoined(game);

            }
        }

        public void JoinGame(string teamName, string gameCode)
        {

        }
        /// <summary>
        /// Gets a Noun from Database Table Nouns and Converts it into a Charade.
        ///  Pushes to Client-side
        /// </summary>
        public void GetCharade(string gameCode)
        {
            string noun = GameState.Instance.GetNoun(gameCode);
            Clients.All.InsertCharadeHTML(noun, "noun");
        }

        public void UpdateCharade(string typeOfWord, string gameCode)
        {
            if (typeOfWord == "adjective")
            {
                string adjective = GameState.Instance.GetAdjective(gameCode);
                Clients.All.InsertCharadeHTML(_charade, "adjective");
            }
            if (typeOfWord == "verb")
            {
                _charade.Verb.Add(_dbUtils.GetVerb().Description);
                Clients.All.InsertCharadeHTML(_charade, "verb");
            }
        }
        #region gamla ordhämtningar
        /// <summary>
        /// Hämtar ett slumpat Substantiv från Databasen
        /// Hämtar även 3 felaktiga alternativ för gissande
        /// </summary>
        //public void GetNoun(string gameCode)
        //{
        //    string noun = GameState.Instance.GetNoun(gameCode);

        //    //Clients.All.incorrectGuesses(tmpList);
        //    Clients.All.InsertCharadeHTML(noun, "noun");
        //}
        /// <summary>
        /// Hämtar slumpat Adjective från databasen
        ///  + 3 felaktiga alternativ för gissande
        /// </summary>
        //public void GetAdjective()
        //{
        //    //string adjective = GameState.Instance.GetAdjective();
        //    ////Clients.All.incorrectGuesses(tmpList);
        //    //Clients.All.InsertCharadeHTML(adjective, "adjective");
        //}
        ///// <summary>
        ///// Hämtar slumpat verb från databasen
        /////  + 3 felaktiga alternativ för gissande
        ///// </summary>
        //public void GetVerb()
        //{
        //    string verb = GameState.Instance.GetVerb();
        //    //Clients.All.incorrectGuesses(tmpList);
        //    Clients.All.InsertCharadeHTML(verb, "verb");
        //}
        #endregion

    }
}