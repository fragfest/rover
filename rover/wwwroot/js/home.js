var home = new Vue({
    el: '#home',
    data: {
        rover: {
            x: 0,
            y: 0,
            startX: 0,
            startY: 0,
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
        this.rover.y = this.grid.height - 1;
        this.rover.startY = 0;
    },

    methods: {
        updateRover: function () {
            //TODO validate inputs > 0 && < grid width, height

            var startY = parseInt(this.rover.startY);
            var startX = parseInt(this.rover.startX);

            this.rover.y = this.grid.height - startY - 1;
            this.rover.x = startX;
        },

        isRoverCell: function (x, y) {
            if (this.rover.x === x && this.rover.y === y) {
                return true;
            }
            return false;
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