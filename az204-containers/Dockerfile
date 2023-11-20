FROM	node:9-alpine
#ADD	https://github.com/Azure-Samples/acr-build-helloworld-node/package.json /
#ADD	https://github.com/Azure-Samples/acr-build-helloworld-node/server.js /
ADD	package.json /
ADD	server.js /
RUN	npm install
EXPOSE	80
CMD	["node", "server.js"]
