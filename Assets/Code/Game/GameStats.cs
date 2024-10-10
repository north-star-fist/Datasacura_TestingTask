namespace Datasacura.TestTask.ZooWorld
{
    public readonly struct GameStats
    {
        public int AlivePrey => _alivePrey;
        private readonly int _alivePrey;

        public int AlivePredators => _alivePredators;
        private readonly int _alivePredators;

        public int DeadPrey => _deadPrey;
        private readonly int _deadPrey;

        public int DeadPredators => _deadPredators;
        private readonly int _deadPredators;

        public GameStats(int alivePredators, int alivePrey, int deadPredators, int deadPrey)
        {
            _alivePredators = alivePredators;
            _alivePrey = alivePrey;
            _deadPredators = deadPredators;
            _deadPrey = deadPrey;
        }

        public GameStats WithAlivePredators(int alivePredators)
        {
            return new(alivePredators, _alivePrey, _deadPredators, _deadPrey);
        }

        public GameStats WithAlivePrey(int alivePrey)
        {
            return new(_alivePredators, alivePrey, _deadPredators, _deadPrey);
        }

        public GameStats WithDeadPredators(int deadPredators)
        {
            return new(_alivePredators, _alivePrey, deadPredators, _deadPrey);
        }

        public GameStats WithDeadPrey(int deadPrey)
        {
            return new(_alivePredators, _alivePrey, _deadPredators, deadPrey);
        }
    }
}
