$(document).ready(function () {

	StartScreen()


	function StartScreen()
	{
		$("#username").show()
		$("#Play").show()
		$("#sectionBox").show()
		$("#Title").show()
		$("#BoxInfo").show()

		$("#FireBtn").hide()
		$("#ReloadBtn").hide()
		$("#bulletImg").hide()
		$("#AmmoCount").hide()


		$("#Play").click(function(){

			if($("#username").val() != "")
				ChangeScene();
		});
	}

	function ChangeScene()
	{
		$("#username").hide()
		$("#Play").hide()
		$("#sectionBox").hide()
		$("#Title").hide()
		$("#BoxInfo").hide()

		$("#FireBtn").show()
		$("#ReloadBtn").show()
		$("#bulletImg").show()
		$("#AmmoCount").show()

		var PlayerId = 0;
	var PlayerTeam = 0;

	var AmmoCount = 5;
	$("#AmmoCount").text(AmmoCount);

	Joingame();

	function Joingame()
	{
		console.log($("#username").val());
		var sendInfo = {
			Name: $("#username").val()
		};

		$.ajax({
		  type: "POST",
		  url: '/join',
		  dataType: "json",
		  data: sendInfo,
		  success: function (msg) {
               PlayerId = msg.Id;
               PlayerTeam = msg.Team;
           }
		});
	}

	window.onbeforeunload = function() {

		var sendInfo = {
			Id: PlayerId
		};

     	$.ajax({
		  type: "POST",
		  url: '/remove',
		  dataType: "json",
		  data: sendInfo
		});
	}
 

	$("#FireBtn").click(function(){

		var sendInfo = {
			Id: PlayerId,
			Team: PlayerTeam,
			Name: $("#username").val(),
			Fired: 1
		};

		if(AmmoCount > 0)
		{
			PostData(sendInfo,'/fireEvent');
			AmmoCount--;
			$("#AmmoCount").text(AmmoCount);
		}


	});

	$("#ReloadBtn").click(function(){
		AmmoCount = 5;
		$("#AmmoCount").text(AmmoCount);
	});

	function PostData(data, url)
	{
		$.ajax({
		  type: "POST",
		  url: url,
		  dataType: "json",
		  data: data,
		  success: function (msg) {
               console.log(msg);
           }
		});
	}

	}




	

 });

