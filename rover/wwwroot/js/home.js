var home = new Vue({
    el: '#home',
    data: {
        //TODO get urls from server using page first page load. on server ideally in a config file
        urlWebSvcRoverPath: 'https://localhost:5003/roverpath',
        urlWebSvcRoverImage: 'https://localhost:5003/roverimages',
        urlWebSvcMission: 'https://localhost:5003/mission',

        disableMovementButtons: false,
        directions: ['North', 'South', 'East', 'West'],
        errorMsg: '',

        showNewRover: false,
        rovers: [],

        rover: {
            x: 0,
            y: 0,
            dir: 'N',
            input: '',
            path: [],

            /* INPUTS which need cast */
            startX: 0,
            startY: 0,
            startDir: 'North',
        },

        grid: {
            maxDimension: 30,
            cellPx: 30,
            widthPx: 0,
            width: 6,
            height: 6,

            /* INPUTS which need cast */
            widthInput: 6,
            heightInput: 6,
        },

        reports: [],
        isMissionInProgress: false,
        errorMsgMission: '',
        disableMissionButton: false,
        showMissionResult: false,
        screenshot: '',
    },

    mounted: function () {
        // origin (0,0) is defined in the bottom left (not top left)

        this.updateRoverStart();
    },

    methods: {

        /////////////////////////////////////////////////////////////////////
        // Add, Save rover
        /////////////////////////////////////////////////////////////////////
        isDisabledDimensions: function () {
            return this.rovers.length > 0 || this.showNewRover;
        },

        addRover: function () {
            var gridYmaxIndex = this.grid.height ? (this.grid.height - 1) : 0;

            this.rover.x = 0;
            this.rover.y = gridYmaxIndex;
            this.rover.dir = 'N';
            this.rover.input = '';
            this.rover.path = [];
            this.rover.startX = 0;
            this.rover.startY = 0;
            this.rover.startDir = 'North';

            this.showNewRover = true;
        },

        saveRover: function () {
            var newRover = {
                x: this.rover.x,
                y: this.rover.y,
                dir: this.rover.dir,
                input: this.rover.input,
                path: this.rover.path,
                startX: parseInt(this.rover.startX) || 0,
                startY: parseInt(this.rover.startY) || 0,
                startDirChar: this.dirStrToChar(this.rover.startDir || ''),
            };
            this.rovers.push(newRover);

            this.showNewRover = false;
        },

        /////////////////////////////////////////////////////////////////////
        // Build Path, input recording
        /////////////////////////////////////////////////////////////////////

        buildPathFromServer: function (pathServer) {
            var dirStrToChar = this.dirStrToChar;
            var gridHeight = this.grid.height;
            var path = [];

            pathServer.forEach(function (pointServer) {
                path.push({
                    x: pointServer.x,
                    y: gridHeight - 1 - pointServer.y,
                    dir: dirStrToChar(pointServer.dir)
                });
            });

            var last = path[path.length - 1];
            this.rover.x = last.x;
            this.rover.y = last.y;
            this.rover.dir = last.dir;
            this.rover.path = path;
        },

        updateRoverPath: function (newChar) {
            var ref = this;
            ref.disableMovementButtons = true;
            ref.errorMsg = '';
            var dirStrToChar = ref.dirStrToChar;

            var roverInput = (ref.rover.input || '') + newChar;
            var startX = parseInt(ref.rover.startX) || 0;
            var startY = parseInt(ref.rover.startY) || 0;
            var startDir = ref.rover.startDir || '';
            var gridWidth = parseInt(ref.grid.width) || 0;
            var gridHeight = parseInt(ref.grid.height) || 0;

            $.ajax({
                url: ref.urlWebSvcRoverPath,
                data: JSON.stringify({
                    input: roverInput,
                    startX: startX,
                    startY: startY,
                    startDir: dirStrToChar(startDir),
                    gridWidth: gridWidth,
                    gridHeight: gridHeight
                }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                type: 'POST',
                success: function (data, status, xhr) {

                    ref.disableMovementButtons = false;
                    ref.rover.input = data.input || '';
                    ref.buildPathFromServer(data.path || []);

                },
                error: function (xhr) {

                    ref.disableMovementButtons = false;
                    console.error('updateRoverPath status ' + xhr.status + ': ' + xhr.responseText);
                    ref.errorMsg = 'An error occurred';

                }
            });
        },

        /////////////////////////////////////////////////////////////////////
        // Build Grid
        /////////////////////////////////////////////////////////////////////

        isPathCell: function (x, y) {
            var match = point => {
                return point.x === x && point.y === y;
            };

            var isStart = () => {
                var startX = this.rover.startX || 0;
                var startY = this.rover.startY || 0;
                var gridYmaxIndex = this.grid.height ? (this.grid.height - 1) : 0;

                return parseInt(startX) === x &&
                    parseInt(startY) === gridYmaxIndex - y;
            };

            return this.rover.path.find(match) || isStart();
        },

        isRoverCell: function (x, y) {
            if (this.rover.x === x && this.rover.y === y) {
                return true;
            }
            return false;
        },

        isNextCellRight(x, y) {
            if (this.rover.x + 1 === x
                && this.rover.y === y
                && this.rover.dir === 'E') {
                    return true;
            }
            return false;
        },

        isNextCellLeft(x, y) {
            if (this.rover.x - 1 === x
                && this.rover.y === y
                && this.rover.dir === 'W') {
                return true;
            }
            return false;
        },

        isNextCellUp(x, y) {
            if (this.rover.x === x
                && this.rover.y - 1 === y
                && this.rover.dir === 'N') {
                return true;
            }
            return false;
        },

        isNextCellDown(x, y) {
            if (this.rover.x === x
                && this.rover.y + 1 === y
                && this.rover.dir === 'S') {
                return true;
            }
            return false;
        },

        /////////////////////////////////////////////////////////////////////
        // Update
        /////////////////////////////////////////////////////////////////////

        updateRoverStart: function () {
            var startY = parseInt(this.rover.startY) || 0;
            var startX = parseInt(this.rover.startX) || 0;
            var gridYmaxIndex = this.grid.height ? (this.grid.height - 1) : 0;
            var gridXmaxIndex = this.grid.width ? (this.grid.width - 1) : 0;

            if (startY <= 0) {
                this.rover.startY = 0;
                startY = 0;
            }
            if (startY > gridYmaxIndex) {
                this.rover.startY = gridYmaxIndex;
                startY = gridYmaxIndex;
            }
            if (startX <= 0) {
                this.rover.startX = 0;
                startX = 0;
            }
            if (startX > gridXmaxIndex) {
                this.rover.startX = gridXmaxIndex;
                startX = gridXmaxIndex;
            }

            this.rover.y = gridYmaxIndex - startY;
            this.rover.x = startX;
            this.rover.dir = this.dirStrToChar(this.rover.startDir);
            this.rover.input = "";
            this.rover.path = [];
        },

        updateGridInput: function () {
            var widthNum = parseInt(this.grid.widthInput) || 0;
            var heightNum = parseInt(this.grid.heightInput) || 0;
            if (widthNum < 0) {
                this.grid.widthInput = 0;
                widthNum = 0;
            }
            if (heightNum < 0) {
                this.grid.heightInput = 0;
                heightNum = 0;
            }
            if (widthNum > this.grid.maxDimension) {
                this.grid.widthInput = this.grid.maxDimension;
                widthNum = this.grid.maxDimension;
            }
            if (heightNum > this.grid.maxDimension) {
                this.grid.heightInput = this.grid.maxDimension;
                heightNum = this.grid.maxDimension;
            }

            this.grid.width = widthNum;
            this.grid.widthPx = widthNum * this.grid.cellPx;
            this.grid.height = heightNum;

            this.rover.startX = 0;
            this.rover.startY = 0;
            this.updateRoverStart();
        },

        dirStrToChar: function (dirStr) {
            switch (dirStr) {
                case 'North':
                    return 'N'
                case 'South':
                    return 'S';
                case 'East':
                    return 'E';
                case 'West':
                    return 'W';
                default:
                    return 'N';
            }
        },

        /////////////////////////////////////////////////////////////////////
        // Send mission, get screenshot
        /////////////////////////////////////////////////////////////////////

        isDisabledMission: function () {
            return this.disableMovementButtons || this.disableMissionButton || !this.rovers.length;
        },

        createReport: function (missionRes) {
            var gridWidth = missionRes.gridWidth || 0;
            var gridHeight = missionRes.gridHeight || 0;
            var rovers = missionRes.rovers || [];
            this.reports = rovers;
        },

        launchMission: function () {
            this.createMissionServer()
                .then(this.getScreenshotServer);
        },

        /* createMissionServer */
        createMissionServer: function() {
            var ref = this;

            return new Promise(function (resolve, reject) {

                ref.disableMissionButton = true;
                ref.isMissionInProgress = true;
                ref.errorMsgMission = '';

                var rovers = ref.rovers || [];
                var gridWidth = parseInt(ref.grid.width) || 0;
                var gridHeight = parseInt(ref.grid.height) || 0;

                $.ajax({
                    url: ref.urlWebSvcMission,
                    data: JSON.stringify({
                        gridWidth: gridWidth,
                        gridHeight: gridHeight,
                        rovers: rovers,
                    }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    type: 'POST',
                    success: function (data, status, xhr) {

                        ref.isMissionInProgress = false;
                        ref.showMissionResult = true;
                        ref.createReport(data);

                        resolve();
                    },
                    error: function (xhr) {

                        ref.isMissionInProgress = false;
                        ref.disableMissionButton = false;
                        ref.errorMsgMission = 'An error occurred';
                        console.error('createMissionServer status ' + xhr.status + ': ' + xhr.responseText);

                        reject();
                    }
                });

            });
        },
        /* createMissionServer END */


        /* getScreenshotServer */
        getScreenshotServer: function() {
            var ref = this;

            return new Promise( function(resolve, reject) {

                var dirStrToChar = ref.dirStrToChar;
                ref.disableMissionButton = true;
                ref.isMissionInProgress = true;
                ref.errorMsgMission = '';

                var roverInput = ref.rover.input || '';
                var startX = parseInt(ref.rover.startX) || 0;
                var startY = parseInt(ref.rover.startY) || 0;
                var startDir = ref.rover.startDir || '';
                var gridWidth = parseInt(ref.grid.width) || 0;
                var gridHeight = parseInt(ref.grid.height) || 0;

                $.ajax({
                    url: ref.urlWebSvcRoverImage,
                    data: {
                        input: roverInput,
                        startX: startX,
                        startY: startY,
                        startDir: dirStrToChar(startDir),
                        gridWidth: gridWidth,
                        gridHeight: gridHeight
                    },
                    type: 'GET',
                    success: function (data, status, xhr) {

                        ref.isMissionInProgress = false;
                        ref.showMissionResult = true;
                        ref.screenshot = 'data:image/png;base64, ' + data;

                        resolve();
                    },
                    error: function (xhr) {

                        ref.isMissionInProgress = false;
                        ref.disableMissionButton = false;
                        ref.errorMsgMission = 'An error occurred';
                        console.error('getScreenshotServer status ' + xhr.status + ': ' + xhr.responseText);

                        reject();
                    }
                });

            });
        },
        /* getScreenshotServer END */

    }

}); 