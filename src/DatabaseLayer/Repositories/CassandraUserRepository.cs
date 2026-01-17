using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using CommonLayer.DTOs;
using CommonLayer.Interfaces;
using MyApp.CommonLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class CassandraUserRepository : IUserService
    {
        private readonly ISession _session;

        private PreparedStatement? _getUserByIdStatement;
        private PreparedStatement? _getUserStats;
        private PreparedStatement? _createUserStatement;
        private PreparedStatement? _initializeUserStatsStatement;
        private PreparedStatement? _updateUserStatement;
        private PreparedStatement? _incrementPlayedGamesStatement;
        private PreparedStatement? _incrementWinsLikeCrewmateStatement;
        private PreparedStatement? _incrementWinsLikeImpostorStatement;
        private PreparedStatement? _addPointsStatement;

        public CassandraUserRepository(ISession session)
        {
            _session = session;
            InitializePreparedStatements();
        }

        private void InitializePreparedStatements()
        {
            _getUserByIdStatement = _session.Prepare("SELECT user_id, username, email " +
                "FROM users WHERE user_id = ?");

            _getUserStats = _session.Prepare("SELECT user_id, games_played, wins_as_crewmate, wins_as_impostor, total_score " +
                "FROM user_stats WHERE user_id = ?");

            _createUserStatement = _session.Prepare("INSERT INTO users (user_id, username, email) " +
                "VALUES (?, ?, ?)");


            _incrementPlayedGamesStatement = _session.Prepare("UPDATE user_stats SET games_played = games_played + 1 WHERE user_id = ?");

            _incrementWinsLikeCrewmateStatement = _session.Prepare("UPDATE user_stats SET wins_as_crewmate = wins_as_crewmate + 1 WHERE user_id = ?");

            _incrementWinsLikeImpostorStatement = _session.Prepare("UPDATE user_stats SET wins_as_impostor = wins_as_impostor + 1 WHERE user_id = ?");

            _addPointsStatement = _session.Prepare("UPDATE user_stats SET total_score = total_score + ? WHERE user_id = ?");

            //_updateUserStatement = _session.Prepare("UPDATE users SET username = ?" +
            //    ", email = ?, " +
            //    "games_played = ?, " +
            //    "wins_as_crewmate = ?, " +
            //    "wins_as_impostor = ?, " +
            //    "total_score = ? " +
            //    "WHERE user_id = ?");
        }

       

        private GetUserResponse? MapRowsToUser(Row? userRow, Row? statsRow)
        {
            if (userRow == null) return null;
            return new GetUserResponse
            {
                UserId = userRow.GetValue<string>("user_id"),
                Username = userRow.GetValue<string>("username"),
                Email = userRow.GetValue<string>("email"),
                GamesPlayed = statsRow?.GetValue<long?>("games_played") ?? 0,
                WinsLikeCrewmate = statsRow?.GetValue<long?>("wins_as_crewmate") ?? 0,
                WinsLikeImpostor = statsRow?.GetValue<long?>("wins_as_impostor") ?? 0,
                TotalScore = statsRow?.GetValue<long?>("total_score") ?? 0
            };
        }

        public Task CreateAsync(CreateUserInput user)
        {
            var boundStatement = _createUserStatement!.Bind(
                user.UserId,
                user.Username,
                user.Email);

            return _session.ExecuteAsync(boundStatement);
        }

        public GetUserResponse? GetUserById(string userId)
        {
            var boundStatement = _getUserByIdStatement!.Bind(userId);
            var userRow = _session.Execute(boundStatement);
            var statsRow = _session.Execute(_getUserStats!.Bind(userId));
            return MapRowsToUser(userRow.FirstOrDefault(), statsRow.FirstOrDefault());
        }

        public Task UpdateAsync(User user)
        {
            //var boundStatement = _updateUserStatement!.Bind(
            //    user.Username,
            //    user.Email,
            //    user.GamesPlayed,
            //    user.WinsLikeCrewmate,
            //    user.WinsLikeImpostor,
            //    user.TotalScore,
            //    user.UserId);

            //return _session.ExecuteAsync(boundStatement);
            throw new NotImplementedException("Nije implementiran update jos uvek");
        }

        public Task IncrementPlayedGames(string userId)
        {
            var boundStatement = _incrementPlayedGamesStatement!.Bind(userId);
            return _session.ExecuteAsync(boundStatement);
        }

        public Task IncrementWinsLikeCrewmate(string userId)
        {
            var boundStatement = _incrementWinsLikeCrewmateStatement!.Bind(userId);
            return _session.ExecuteAsync(boundStatement);
        }

        public Task IncrementWinsLikeImpostor(string userId)
        {
            var boundStatement = _incrementWinsLikeImpostorStatement!.Bind(userId);
            return _session.ExecuteAsync(boundStatement);
        }

        public Task AddPoints(string userId, long points)
        {
            var boundStatement = _addPointsStatement!.Bind(points, userId);
            return _session.ExecuteAsync(boundStatement);
        }
    }
}
