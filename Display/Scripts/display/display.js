(function ($) {
    $.fn.display = function (options) {
        return this.each(function () {
            var $this = $(this);

            var opt = $.extend({}, { "url": null, "interval": 3600000, "maximage": null }, options, $this.data());

            if (!opt.url) {
                $this.html($("<div/>", { "class": "error" }).html("Missing required option: url"));
                return;
            }

            var _display = null;

            var needsRefresh = function (d1, d2) {
                var m1 = moment(d1.lastUpdate);
                var m2 = moment(d2.lastUpdate);
                return m2.isAfter(m1);
            };

            var createPlugin = function () {
                if (!_display) return;

                var mi = $("<div/>", { "class": "maximage" });

                mi.html($.map(_display.images, function (value, index) {
                    console.log(value);
                    return $("<img/>", { "src": value.url, "alt": "" });
                }));

                $this.html(mi);

                mi.maximage(opt.maximage);
            };

            var refresh = function () {
                $.ajax({
                    "url": opt.url
                }).done(function (data, textStatus, jqXHR) {
                    if (data.images.length === 0) {
                        _display = null;
                        $this.html($("<div/>", { "class": "error" }).html("No images found."));
                    } else if (_display === null) {
                        //only recreate the slide show if the image array has changed.
                        _display = data;
                        createPlugin();
                    } else {
                        if (needsRefresh(_display, data))
                            window.location.reload(true);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    _display = null;

                    var errmsg = jqXHR.responseJSON && jqXHR.responseJSON.error
                        ? jqXHR.responseJSON.error
                        : errorThrown;

                    $this.html($("<div/>", { "class": "error" }).html(errmsg));
                });
            };

            setTimeout(refresh(), 500);

            if (opt.interval) {
                setInterval(function () {
                    refresh();
                }, opt.interval);
            }
        });
    };
}(jQuery));