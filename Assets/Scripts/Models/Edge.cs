using Unity.Mathematics;

namespace Models
{
    public class Edge
    {
        private Point _start;
        private Point _end;
        
        public Point Start
        {
            get => _start;
            set
            {
                _start = value;
                Length = GetLength();
            }
        }

        public Point End
        {
            get => _end;
            set
            {
                _end = value;
                Length = GetLength();
            }
        }

        public float Length { get; private set; }

        public Edge(Point start, Point end)
        {
            _start = start;
            _end = end;
            Length = GetLength();
        }

        private float GetLength()
        {
            var dx = End.x - Start.x;
            var dy = End.y - Start.y;
            return math.sqrt(dx * dx + dy * dy);
        }
    }
}