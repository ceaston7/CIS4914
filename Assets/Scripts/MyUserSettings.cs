namespace MyUserSettings{
    public enum Locomotion{
        blink,
        slide,
        walk
    }

    public class MyUserSettings
    {
        //Mode of movement choice
        public static Locomotion LocomotionMode;

        //Height of tracker above playspace floor when standing normally
        public static float baseFootHeight;
    }
}