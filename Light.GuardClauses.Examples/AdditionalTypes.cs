using System;

namespace Light.GuardClauses.Examples
{
    public interface IMovie
    {
        void SetMovieRating(int numberOfStars);
    }

    public interface IMovieRepository
    {
        IMovie GetById(Guid id);
    }

    public class CustomerInfo
    {
        public bool IsComplete => false;
    }

    public interface ICustomerRepository { }

    public class StupidTeamMembersException : Exception
    {
        public StupidTeamMembersException(string message)
            : base(message)
        {
            
        }
    }
}