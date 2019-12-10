FROM abiosoft/caddy

COPY ./dist app/
COPY ./Caddyfile /etc/Caddyfile
WORKDIR app
RUN ls
EXPOSE 80
CMD ["--conf", "/etc/Caddyfile", "--log", "stdout"]