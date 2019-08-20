FROM node:10.9-slim

# not mandatory if you EXPORT a fixed port. See below.
ARG YOUR_APP_WEB_HTTP_PORT

RUN apt-get -y update \
	&& apt-get install -y git

RUN yarn global add @vue/cli -g

WORKDIR /wwwroot

RUN apt-get autoremove -y \
    && apt-get autoclean -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 8080

USER node

# switch to npm if you chose it as package manager
CMD ["yarn", "serve"]
