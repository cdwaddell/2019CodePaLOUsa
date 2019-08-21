import axiosRetry from 'axios-retry'
import axios from "axios";
import JwtService from "@/common/jwt.service";
import { API_URLS, HOSTS } from "@/common/config";

export const ApiService = {
  init() {
    ArticlesService.init(API_URLS.BLOG_URL);

    CommentsService.init(API_URLS.COMMENT_URL);

    AuthService.init(API_URLS.AUTH_URL);
  },

  setHeader() {
    var token = JwtService.getToken();
    ArticlesService.instance.defaults.headers.common[
      "Authorization"
    ] = `Token ${token}`;
    CommentsService.instance.defaults.headers.common[
      "Authorization"
    ] = `Token ${token}`;
    AuthService.instance.defaults.headers.common[
      "Authorization"
    ] = `Token ${token}`;
  },

  query(instance, resource, params) {
    return instance.get(resource, params).catch(error => {
      throw new Error(`[RWV] ApiService ${error}`);
    });
  },

  get(instance, resource, slug = "") {
    return instance.get(`${resource}/${slug}`).catch(error => {
      throw new Error(`[RWV] ApiService ${error}`);
    });
  },

  post(instance, resource, params) {
    return instance.post(`${resource}`, params);
  },

  update(instance, resource, slug, params) {
    return instance.put(`${resource}/${slug}`, params);
  },

  put(instance, resource, params) {
    return instance.put(`${resource}`, params);
  },

  delete(instance, resource) {
    return instance.delete(resource).catch(error => {
      throw new Error(`[RWV] ApiService ${error}`);
    });
  }
};

export const ArticlesService = {
  instance: {},

  init(url) {
    this.instance = axios.create({
      baseURL: url,
      headers: { "X-Custom-Header": "foobar" }
    });
    axiosRetry(this.instance, { retries: 3, shouldResetTimeout: true, retryDelay: axiosRetry.exponentialDelay });
  },

  query(type, params) {
    return ApiService.query(
      this.instance,
      "articles" + (type === "feed" ? "/feed" : ""),
      {
        params: params
      }
    );
  },

  get(slug) {
    return ApiService.get(this.instance, "articles", slug);
  },

  create(params) {
    return ApiService.post(this.instance, "articles", {
      article: params
    });
  },

  update(slug, params) {
    return ApiService.update(this.instance, "articles", slug, {
      article: params
    });
  },

  destroy(slug) {
    return ApiService.delete(this.instance, `articles/${slug}`);
  },

  getTags() {
    return ApiService.get(this.instance, "tags");
  },

  addFavorite(slug) {
    return ApiService.post(this.instance, `articles/${slug}/favorite`);
  },

  removeFavorite(slug) {
    return ApiService.delete(this.instance, `articles/${slug}/favorite`);
  },

  follow(username) {
    return ApiService.post(this.instance, `profiles/${username}/follow`);
  },

  getProfile(username) {
    return ApiService.get(this.instance, "profiles", username);
  },

  unfollow(username) {
    return ApiService.delete(this.instance, `profiles/${username}/follow`);
  }
};

export const CommentsService = {
  instance: {},

  init(url) {
    this.instance = axios.create({
      baseURL: url,
      headers: { "X-Custom-Header": "foobar" }
    });
    axiosRetry(this.instance, { retries: 3, shouldResetTimeout: true, retryDelay: axiosRetry.exponentialDelay });
  },

  get(slug) {
    if (typeof slug !== "string") {
      throw new Error(
        "[RWV] CommentsService.get() article slug required to fetch comments"
      );
    }
    return ApiService.get(this.instance, "articles", `${slug}/comments`);
  },

  post(slug, payload) {
    return ApiService.post(this.instance, `articles/${slug}/comments`, {
      comment: { body: payload }
    });
  },

  destroy(slug, commentId) {
    return ApiService.delete(
      this.instance,
      `articles/${slug}/comments/${commentId}`
    );
  }
};

export const AuthService = {
  instance: {},

  init(url) {
    this.instance = axios.create({
      baseURL: url,
      headers: { "X-Custom-Header": "foobar" }
    });
    axiosRetry(this.instance, { retries: 3, shouldResetTimeout: true, retryDelay: axiosRetry.exponentialDelay });
  },
  
  setHeader() {
    return ApiService.setHeader(this.instance);
  },
  login(credentials) {
    return ApiService.post(this.instance, "users/login", { user: credentials });
  },
  users(credentials) {
    return ApiService.post(this.instance, "users", { user: credentials });
  },
  getUser() {
    return ApiService.get(this.instance, "user");
  },
  updateUser(user) {
    return ApiService.put(this.instance, "user", user);
  }
};
