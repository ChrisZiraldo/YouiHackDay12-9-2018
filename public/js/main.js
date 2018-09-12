$(document).ready(function() {

    Selection = 0;

    function HideAll() {
        $("#username").hide()
        $("#Play").hide()
        $("#sectionBox").hide()
        $("#Title1").hide()
        $("#Title2").hide()
        $("#BoxInfo").hide()

        $("#RockBtn").hide()
        $("#PaperBtn").hide()
        $("#ScissorsBtn").hide()
        $("#WaitingText").hide()
        $("#Spinner").hide()
        $("#ResultImage").hide()
        $("#ChoiceText").hide()
        $("#PlayAgain").hide()
        
        
    }

    StartScreen()

    function StartScreen() {
        HideAll()

        $("#username").show()
        $("#Play").show()
        $("#sectionBox").show()
        $("#Title1").show()
        $("#Title2").show()
        $("#BoxInfo").show()

        $("#Play").click(function() {

            if ($("#username").val() != "")
                WaitingScreen();
        });
    }

    function WaitingScreen() {
        HideAll()

        $("#WaitingText").show()
        $("#Spinner").show()


        Joingame();

        GetPlayerCount()
    }

    function ResultsScreen() {
        HideAll()

        $("#ResultImage").show()
        $("#ChoiceText").show()

        if(Selection == 1)
        {
            $("#ChoiceText").text('You Chose Rock!')
            $('#ResultImage').attr('src','https://www.campcanary.co.uk/uploads/rock-icon-grey.png');
        }
        else if(Selection == 2)
        {
            $("#ChoiceText").text('You Chose Paper!')
            $('#ResultImage').attr('src','https://www.campcanary.co.uk/uploads/paper-icon-grey.png');
        }
        else if(Selection == 3)
        {
            $("#ChoiceText").text('You Chose Scissors!')
            $('#ResultImage').attr('src','https://www.campcanary.co.uk/uploads/scissors-icon-grey.png');
        }
        GetGameOver()
    }

    function GetGameOver() {
        $.ajax({
            type: "GET",
            url: '/GetGameOver',
            success: function(msg) {
                if (msg.GameOver == true) {
                    $("#PlayAgain").show()
                } else {
                    console.log("GetGameOver");
                    setTimeout(GetGameOver, 200)
                }
            }
        });
    }

    $("#PlayAgain").click(function() {
        StartScreen();
    });

    function GetPlayerCount() {
        $.ajax({
            type: "GET",
            url: '/GetPlayerCount',
            success: function(msg) {
                if (msg.Count == 2) {
                    GameScene()
                } else {
                    setTimeout(GetPlayerCount, 200)
                }
            }
        });
    }

    function Joingame() {
        console.log($("#username").val());
        var sendInfo = {
            Name: $("#username").val()
        };

        $.ajax({
            type: "POST",
            url: '/join',
            dataType: "json",
            data: sendInfo,
            success: function(msg) {

                PlayerId = msg.Id;
            }
        });
    }

    function GameScene() {
        HideAll()

        $("#RockBtn").show()
        $("#PaperBtn").show()
        $("#ScissorsBtn").show()

        var PlayerId = 0;



        $("#RockBtn").click(function() {
            Selection = 1;

            var sendInfo = {
                Id: PlayerId,
                Choice: Selection
            };

            PostData(sendInfo, '/SubmitResults');
            
            ResultsScreen()

            PostData(sendInfo, '/fireEvent');
        });

        $("#PaperBtn").click(function() {
            Selection = 2;

            var sendInfo = {
                Id: PlayerId,
                Choice: Selection
            };

            PostData(sendInfo, '/SubmitResults');
            
            ResultsScreen()

            
        });

        $("#ScissorsBtn").click(function() {
            Selection = 3;

            var sendInfo = {
                Id: PlayerId,
                Choice: Selection
            };

            PostData(sendInfo, '/SubmitResults');
            
            ResultsScreen()
        });

        function PostData(data, url) {
            $.ajax({
                type: "POST",
                url: url,
                dataType: "json",
                data: data,
                success: function(msg) {
                    console.log(msg);
                }
            });
        }

    }
});