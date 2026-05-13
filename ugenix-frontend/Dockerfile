# Build Stage
FROM node:20-alpine AS build
ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Production Stage
FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx config directly to a temporary location
COPY nginx.conf /etc/nginx/nginx.conf.template

EXPOSE 80

# Use a shell script to extract all nameservers, replace placeholders, and start nginx
CMD ["/bin/sh", "-c", "export NAMESERVERS=$(awk '/^nameserver/ {print $2}' /etc/resolv.conf | tr '\\n' ' ') && sed -i \"s|__API_URL__|${API_URL}|g\" /etc/nginx/nginx.conf.template && sed -i \"s|__NAMESERVERS__|${NAMESERVERS}|g\" /etc/nginx/nginx.conf.template && cp /etc/nginx/nginx.conf.template /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]
