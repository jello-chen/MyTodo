var path = require('path');
var webpack = require('webpack');
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
  entry: './src/index.ts',
  output: {
    path: path.resolve(__dirname, './wwwroot'),
    publicPath: '/wwwroot/',
    filename: 'js/build.js'
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: 'vue-loader',
        options: {
          loaders: {
            // Since sass-loader (weirdly) has SCSS as its default parse mode, we map
            // the "scss" and "sass" values for the lang attribute to the right configs here.
            // other preprocessors should work out of the box, no loader config like this necessary.
            'scss': 'vue-style-loader!css-loader!sass-loader',
            'sass': 'vue-style-loader!css-loader!sass-loader?indentedSyntax',
          }
          // other vue-loader options go here
        }
      },
      {
        test: /\.tsx?$/,
        loader: 'ts-loader',
        exclude: /node_modules/,
        options: {
          appendTsSuffixTo: [/\.vue$/],
        }
      },
      {
        test: /\.(png|jpg|gif|svg)$/,
        loader: 'file-loader',
        options: {
          name: '[name].[ext]?[hash]'
        }
      }
    ]
  },
  resolve: {
    extensions: ['.ts', '.js', '.vue', '.json'],
    alias: {
      'vue$': 'vue/dist/vue.esm.js'
    }
  },
  devServer: {
    historyApiFallback: true,
    noInfo: true
  },
  performance: {
    hints: false
  },
  devtool: '#eval-source-map',
  plugins:[
    new CopyWebpackPlugin([{
      from: path.resolve(__dirname, "./node_modules/todomvc-common/base.css"),
      to: path.resolve(__dirname, "./wwwroot/css/")
    },{
      from: path.resolve(__dirname, "./node_modules/todomvc-app-css/index.css"),
      to: path.resolve(__dirname, "./wwwroot/css/")
    },{
      from: path.resolve(__dirname, "./node_modules/todomvc-common/base.js"),
      to: path.resolve(__dirname, "./wwwroot/js/")
    }])
  ]
}

if (process.env.NODE_ENV === 'production') {
  module.exports.devtool = '#source-map'
  module.exports.plugins = (module.exports.plugins || []).concat([
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: '"production"'
      }
    }),
    new webpack.optimize.UglifyJsPlugin({
      sourceMap: true,
      compress: {
        warnings: false
      }
    }),
    new webpack.LoaderOptionsPlugin({
      minimize: true
    }),
    new CopyWebpackPlugin([{
      from: path.resolve(__dirname, "./node_modules/todomvc-common/base.css"),
      to: path.resolve(__dirname, "./wwwroot/css/")
    },{
      from: path.resolve(__dirname, "./node_modules/todomvc-app-css/index.css"),
      to: path.resolve(__dirname, "./wwwroot/css/")
    },{
      from: path.resolve(__dirname, "./node_modules/todomvc-common/base.js"),
      to: path.resolve(__dirname, "./wwwroot/js/")
    }])
  ])
}