using Aimtec;

namespace Woke.Modules.RecallTracker
{
    internal class Recall
    {
        public float Start;
        public float End;
        public Obj_AI_Hero Sender;
        public float TotalSeconds => End - Start;
        public float SecondsLeft => End - Game.ClockTime;
        public float SecondsPassed => TotalSeconds - SecondsLeft;
        public float Percent => SecondsPassed / TotalSeconds * 100;
        

        public Recall(float start, float end, Obj_AI_Hero sender)
        {
            Start = start;
            End = end;
            Sender = sender;
        }
    }
}
