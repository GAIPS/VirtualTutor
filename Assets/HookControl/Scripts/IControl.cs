namespace HookControl {
    public enum ShowResult
	{
		FIRST,
		OK,
		FAIL
	}

	public interface IControl
	{
		ShowResult Show ();

		void Destroy ();

		void Disable ();

		bool IsVisible ();

	}
}
