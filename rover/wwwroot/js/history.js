var home = new Vue({
    el: '#history',
    data: {
        //TODO get urls from server using first page load. ideally from a config file
        urlWebSvcRoverImage: 'https://localhost:5003/roverimages',
        urlWebSvcMission: 'https://localhost:5003/mission',

        reports: [],
        errorMsgMission: '',
        screenshot: '',
        isInProgress: false,
    },

    mounted: function () {
        // origin (0,0) is defined in the bottom left (not top left)

        this.isInProgress = true;
        this.createMissionServer();
        this.getScreenshotServer();
    },

    methods: {

        createReport: function (missionRes) {
            var gridWidth = missionRes.gridWidth || 0;
            var gridHeight = missionRes.gridHeight || 0;
            var rovers = missionRes.rovers || [];
            this.reports = rovers;
        },

        /* createMissionServer */
        createMissionServer: function() {
            var ref = this;

            return new Promise(function (resolve, reject) {

                ref.errorMsgMission = '';

                $.ajax({
                    url: ref.urlWebSvcMission,
                    type: 'GET',
                    success: function (data, status, xhr) {

                        ref.createReport(data)
                        resolve();
                    },
                    error: function (xhr) {

                        ref.errorMsgMission = 'There is no saved mission';
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

                ref.errorMsgMission = '';

                $.ajax({
                    url: ref.urlWebSvcRoverImage,
                    type: 'GET',
                    success: function (data, status, xhr) {

                        ref.isInProgress = false;
                        ref.screenshot = 'data:image/png;base64, ' + data;
                        resolve();
                    },
                    error: function (xhr) {

                        ref.isInProgress = false;
                        ref.errorMsgMission = 'There is no saved mission';
                        console.error('getScreenshotServer status ' + xhr.status + ': ' + xhr.responseText);
                        reject();
                    }
                });

            });
        },
        /* getScreenshotServer END */

    }

}); 