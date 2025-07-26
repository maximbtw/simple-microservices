FROM node:lts-alpine
RUN npm install -g http-server

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY package*.json ./
COPY tsconfig.json vite.config.ts ./
COPY index.html ./
COPY src ./src

RUN npm run build

EXPOSE 8080
CMD ["http-server", "dist", "-p", "8080", "--spa"]