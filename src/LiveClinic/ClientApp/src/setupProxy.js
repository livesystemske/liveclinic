const {createProxyMiddleware} = require('http-proxy-middleware');
const {env} = require('process');


const context = [
    process.env.REACT_APP_LCS_API_URL, "/bff", "/signin-oidc", "/signout-callback-oidc"
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: process.env.REACT_APP_LCS_BFF,
        secure: false,
        headers: {
            Connection: 'Keep-Alive'
        },
        changeOrigin: true,
        logLevel: "info"
    });

    app.use(appProxy);
};
