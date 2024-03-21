namespace PopupSystem
{
    public static class PopupInstanceTracker
    {
       private  static WorldSpacePopupBase _currentPopupInstance;

       public static WorldSpacePopupBase CurrentPopupInstance
        {
            private get
            {
                // Getter logic
                return _currentPopupInstance;
            }
            set
            {
                if (_currentPopupInstance)
                {
                    _currentPopupInstance.ClosePopup();
                }
                // Setter logic
                _currentPopupInstance = value;
            }
        }
    }
}
