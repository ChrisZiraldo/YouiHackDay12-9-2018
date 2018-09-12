var express = require('express'),
    path = require('path'),
    bodyParser = require('body-parser'),
    cons = require('consolidate'),
    dust = require('dustjs-helpers'),
    app = express();

app.engine('dust', cons.dust);


app.set('view engine', 'dust');
app.set('views', __dirname + '/views');

app.use(express.static(path.join(__dirname, 'public')));

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false}));

app.get('/', function (req, res) {
  res.render('index');
});

var Users = [];
var CurrentID = 0;
var CurrentTeam = 0;

function JoinRoom(name)
{
	Users.push({Id: CurrentID, Team: CurrentTeam, Name: name, Fired: 0});

	CurrentID++;
	if(CurrentTeam == 0)
		CurrentTeam = 1;
	else
		CurrentTeam = 0
}

app.get('/reset', function (req, res) {
  	Users = [];
	CurrentID = 0;
	CurrentTeam = 0;
});

app.post('/join', function (req, res) {
  JoinRoom(req.body.Name);
  res.json({Id: CurrentID-1, Team: CurrentTeam});
});

app.post('/remove', function (req, res) {

	for(var i = 0; i < Users.length; i++)
	{
		if(Users[i].Id == parseInt(req.body.Id))
		{
			Users.splice(i, 1);
		}
	}
 
  res.end('true');
});


app.get('/getInfo', function (req, res) {
  res.send(Users);

	for(var i = 0; i < Users.length; i++)
	{
		Users[i].Fired = 0;
	}
});


app.post('/fireEvent',function(req,res){

	for(var i = 0; i < Users.length; i++)
	{
		if(Users[i].Id == parseInt(req.body.Id))
		{
			Users[i].Fired = parseInt(req.body.Fired)
		}
	}	

	res.end('true');

});

app.listen(3000, function () {
  console.log('Server Started On Port 3000');
});
