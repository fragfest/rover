var home = new Vue({
    el: '#home',
    data: {
        rover: {
            x: 0,
            y: 0,
            dir: 'E',
            startX: 0,
            startY: 0,
            startDir: 'E',
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
        this.rover.y = this.grid.height - 1;
        this.rover.startY = 0;
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

        updateRover: function () {
            //TODO validate inputs > 0 && < grid width, height

            var startY = parseInt(this.rover.startY);
            var startX = parseInt(this.rover.startX);

            this.rover.y = this.grid.height - startY - 1;
            this.rover.x = startX;
        },

        updateGridInput: function () {
            //TODO validate inputs > 0 && < maxDimension

            var widthNum = parseInt(this.grid.widthInput) || 0;
            var heightNum = parseInt(this.grid.heightInput) || 0;

            this.grid.width = widthNum;
            this.grid.widthPx = widthNum * this.grid.cellPx;
            this.grid.height = heightNum;
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