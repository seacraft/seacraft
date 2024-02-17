FROM node:18.13 AS build

# RUN echo 'deb http://mirrors.aliyun.com/debian/ stretch main' > /etc/apt/sources.list \
#     && echo 'deb http://mirrors.aliyun.com/debian/ stretch-updates main' >>/etc/apt/sources.list \
#     && echo 'deb http://mirrors.aliyun.com/debian/ stretch-backports main' >>/etc/apt/sources.list \
#     && apt clean \
#     && apt update \
# 	&& apt install -y build-essential python-dev

WORKDIR /app

COPY . .

RUN npm config set registry https://registry.npmmirror.com/ && \
    npm install -g @angular/cli@latest typescript@latest && \
    npm install
RUN ng build --prod

FROM alpine:3.17 AS remove_exif

RUN sed -i "s@https\?://[^/]*@http://mirrors.aliyun.com@" /etc/apk/repositories && \
    apk update && apk add -U bash exiftool

COPY --from=build /app/dist /app/dist

WORKDIR /app/dist

RUN find . -iname '*.jpg' -o -iname '*.jpeg' -o -iname '*.png' -exec exiftool -all= {} \;

FROM nginx:stable-alpine3.17 AS final

RUN sed -i "s@https\?://[^/]*@http://mirrors.aliyun.com@" /etc/apk/repositories && apk update && apk add -U gettext bash pcre && \
    rm /etc/nginx/conf.d/*

WORKDIR /app

COPY --from=roquie/smalte:latest-alpine /app/smalte /usr/local/bin/smalte
COPY --from=remove_exif /app/dist/ui /var/www/ui
COPY --from=build /app/misc/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/misc/app.conf.template /etc/nginx/conf.d/app.conf.template
COPY --from=build /app/misc/start.sh .

RUN chmod a+x *.sh

ENV WEBAPI__SERVER sea-apiserver.sea:8080
RUN ls
CMD ./start.sh