var home = new Vue({
    el: '#home',
    data: {
        directions: ['North', 'South', 'East', 'West'],
        rover: {
            x: 0,
            y: 0,
            dir: 'N',
            startX: 0,
            startY: 0,
            startDir: 'North',
        },
        grid: {
            maxDimension: 30,
            cellPx: 30,
            widthInput: 10,
            heightInput: 10,
            widthPx: 0,
            width: 10,
            height: 10,
        }
    },
    mounted: function () {
        // origin (0,0) is defined in the bottom left (not top left)

        this.updateRoverStart();
    },

    methods: {

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

        updateRoverStart: function () {
            var startY = parseInt(this.rover.startY) || 0;
            var startX = parseInt(this.rover.startX) || 0;
            if (startY < 0) {
                this.rover.startY = 0;
                startY = 0;
            }
            if (startY > this.grid.height - 1) {
                this.rover.startY = this.grid.height - 1;
                startY = this.grid.height - 1;
            }
            if (startX < 0) {
                this.rover.startX = 0;
                startX = 0;
            }
            if (startX > this.grid.width - 1) {
                this.rover.startX = this.grid.width - 1;
                startX = this.grid.width - 1;
            }

            this.rover.y = this.grid.height - startY - 1;
            this.rover.x = startX;
            this.rover.dir = this.dirStrToChar(this.rover.startDir);
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
            }
        },

        serverGetGrid: function serverGetGrid(width, height) {
            $.ajax({
                url: 'https://localhost:5003/roverimages',
                //data: JSON.stringify({
                //    var1: this.pageUrl,
                //}),
                //dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                type: 'GET',
                success: function (data, status, xhr) {
                    console.log('SUCCESS')
                    //resolve();
                },
                error: function (xhr) {
                    console.error('FAIL')
                    //reject(new Error('save status ' + xhr.status + ': ' + xhr.responseText));
                }
            });
        }

    }

});