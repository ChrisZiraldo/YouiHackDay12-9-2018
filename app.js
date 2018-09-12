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
app.use(bodyParser.urlencoded({
    extended: false
}));

app.get('/', function(req, res) {
    res.render('index');
});

var Users = [];
var CurrentID = 0;
var CurrentTeam = 0;

function ResetRoom() {
Users = [];
    CurrentID = 0;
}

app.get('/CreateRoom', function(req, res) {
    ResetRoom();
    res.end('true');
});

app.get('/GetPlayerInfo', function(req, res) {
    var userinfo = [];

    for (var i = 0; i < Users.length; i++) {
        userinfo.push({
            Id: Users[i].Id,
            Name: Users[i].Name
        });
    }

    res.send(userinfo);
});

app.get('/GetPlayerCount', function(req, res) {
    res.send({Count: Users.length});
});

app.get('/GetGameOver', function(req, res) {
    if(Users.length > 0)
    {
        res.send({GameOver: false});
    }
    else
    {
        res.send({GameOver: true});
    }
});

app.post('/SubmitResults', function(req, res) {

    for (var i = 0; i < Users.length; i++) {
        if (Users[i].Id == parseInt(req.body.Id)) {
            Users[i].Choice = parseInt(req.body.Choice)
        }
    }

    console.log(Users);

    res.end('true');
});

app.get('/GetResults', function(req, res) {
    var userinfo = [];

    for (var i = 0; i < Users.length; i++) {
        userinfo.push({
            Id: Users[i].Id,
            Choice: Users[i].Choice
        });
    }

    res.send(userinfo);
});

app.get('/FakeGetPlayerInfo', function(req, res) {
    var userinfo = [];

    userinfo.push({
        Id: 0,
        Name: "Chris"
    });
    userinfo.push({
        Id: 1,
        Name: "Dave"
    });
 
    res.send(userinfo);
});

app.get('/FakeGetResults', function(req, res) {
    var userinfo = [];

    userinfo.push({
        Id: 0,
        Choice: 0
    });
    userinfo.push({
        Id: 1,
        Choice: 0
    });

    res.send(userinfo);
});


app.get('/DestroyRoom', function(req, res) {
    ResetRoom();
    res.end('true');
});

app.post('/join', function(req, res) {
    JoinRoom(req.body.Name);
    res.json({
        Id: CurrentID - 1,
        Name: req.body.Name
    });
});

function JoinRoom(name) {
    Users.push({
        Id: CurrentID,
        Name: name,
        Choice: 0
    });
    CurrentID++;
    console.log("JOined");
}

app.listen(3000, function() {
    console.log('Server Started On Port 3000');
});