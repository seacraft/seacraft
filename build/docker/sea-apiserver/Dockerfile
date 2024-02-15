# Copyright 2024 The seacraft Authors. All rights reserved.
# Use of this source code is governed by an Apache license
# that can be found in the LICENSE file.

FROM BASE_IMAGE
LABEL maintainer="<info@seacraft.io>"

WORKDIR /opt/seacraft

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && \
      echo "Asia/Shanghai" > /etc/timezone && \
      mkdir -p /var/log/seacraft

COPY sea-apiserver /opt/seacraft/bin/

ENTRYPOINT ["/opt/seacraft/bin/sea-apiserver"]
CMD ["-c", "/etc/seacraft/sea-apiserver.yaml"]