
window.messagesScroll = {
    save: function () {
        sessionStorage.setItem(
            "messagesScrollY",
            window.scrollY.toString()
        );
    },

    restore: function () {
        const savedPosition =
            sessionStorage.getItem("messagesScrollY");

        if (savedPosition !== null) {
            window.scrollTo(0, parseInt(savedPosition, 10));
            sessionStorage.removeItem("messagesScrollY");
        }
    }
};
