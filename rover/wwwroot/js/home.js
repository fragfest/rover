var home = new Vue({
    el: '#home',
    data: {
        message: 'Hello Vue!'
    },
    mounted: function () {
        console.log('VUE mounted');
        this.serverGetGrid(10, 10);
    },

    methods: {
        serverGetGrid: function serverGetGrid(width, height) {
            $.ajax({
                url: 'https://localhost:5003/roverimages',
                //data: JSON.stringify({
                //    var1: this.pageUrl,
                //}),
                //dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                type: 'GET',
                headers: {},
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