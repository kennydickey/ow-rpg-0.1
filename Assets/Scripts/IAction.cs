namespace RPG.Core
{

    public interface Iaction
    {
        // just a contract, nothing actually implemented here
        // only things that can be implemented in other classes may go here methods & properties, no vars
        void Cancel(); // <<- any class that implements this interface has to have

    }
}