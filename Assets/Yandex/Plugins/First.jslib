mergeInto(LibraryManager.library, {
    Hello: function() {
        window.alert("Hello, world!");
    },

    AddScoreExtern: function(value) {
        ysdk.adv.showRewardedVideo({
            callbacks: {
                onOpen: () => {
                    console.log('Video ad open.');
                },
                onRewarded: () => {
                    console.log('Rewarded!');
                    myGameInstance.SendMessage("Canvas Score/Score Monitor/Resource UI", "AddScore", value);
                },
                onClose: () => {
                    console.log('Video ad closed.');
                },
                onError: (e) => {
                    console.log('Error while open video ad:', e);
                }
            }
        });
    },
    ShowAd: function(){
      ysdk.adv.showFullscreenAdv({
        callbacks: {
          onClose: function(wasShown) {
          // some action after close
          },
          onError: function(error) {
          // some action on error
          }
        }
      });
    },

});