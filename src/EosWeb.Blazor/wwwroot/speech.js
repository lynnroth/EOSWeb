window.speechFunctions = {
    startSpeech: function(dotnetObject) {
        var recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition || window.mozSpeechRecognition || window.msSpeechRecognition)();
        recognition.lang = 'en-US';
        recognition.interimResults = false;
        recognition.maxAlternatives = 5;
        recognition.start();

        recognition.onresult = function (event) {
            console.log('You said: ', event.results[0][0].transcript);
            speechFunctions.sendSpeech(event.results[0][0].transcript)
        };
    },

    sendSpeech: function (text) {
        //return dotnetHelper.invokeMethodAsync('SendSpeech', text)
        //    .then(r => console.log(r));
        return DotNet.invokeMethodAsync('EosWeb.Blazor', 'Send', text)
            .then(data => { console.log(data) });
    }
};
