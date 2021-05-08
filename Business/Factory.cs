namespace DSED05.Business
{

    public class Factory
    {
        //Decides which punter to instatiate 0,1,2

        public static Punter GetAPunter(int id)
        {
            switch (id)
            {
                case 0:
                    return new Jack();
                case 1:
                    return new Vaughn();
                case 2:
                    return new Jeremy();
                default:
                    return null;
            }
        }

    }

}
