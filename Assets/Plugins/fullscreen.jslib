mergeInto(LibraryManager.library, {
  SetFullscreen: function (isFullscreen) {
    if (isFullscreen)
      unityInstance.SetFullscreen(1)
    else
      unityInstance.SetFullscreen(0)
  }
});