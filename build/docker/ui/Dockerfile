FROM alpine:3.17 AS filter

RUN sed -i "s@https\?://[^/]*@http://mirrors.aliyun.com@" /etc/apk/repositories && \
    apk update && apk add -U bash exiftool

COPY /ui /app

WORKDIR /app/dist

RUN find . -iname '*.jpg' -o -iname '*.jpeg' -o -iname '*.png' -exec exiftool -all= {} \;

FROM nginx:stable-alpine3.17 AS final

RUN sed -i "s@https\?://[^/]*@http://mirrors.aliyun.com@" /etc/apk/repositories && apk update && apk add -U gettext bash pcre && \
    rm /etc/nginx/conf.d/*

WORKDIR /app

COPY --from=roquie/smalte:latest-alpine /app/smalte /usr/local/bin/smalte
COPY --from=filter /app/dist /var/www/ui
COPY --from=filter /app/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=filter /app/nginx/app.conf.template /etc/nginx/conf.d/app.conf.template
COPY --from=filter /app/nginx/start.sh .

RUN chmod a+x *.sh

ENV WEBAPI__SERVER sea-apiserver.sea:8080
RUN ls
CMD ./start.sh