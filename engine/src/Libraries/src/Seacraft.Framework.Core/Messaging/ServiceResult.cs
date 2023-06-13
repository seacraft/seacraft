// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using Seacraft.Framework.Core.Extensions;


namespace Seacraft.Framework.Core.Messaging
{
    /// <summary>
    /// Api unified response result class
    /// </summary>
    /// <typeparam name="T">the type of the data</typeparam>
    public class ServiceResult<T> : IResult
    {
        /// <summary>
        /// The Response data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// The Response state
        /// </summary>
        public ResultType Status { get; set; }

        /// <summary>
        /// Gets or sets the return message
        /// </summary>
        public string Message
        {
            get { return _message ?? Status.ToDescription(); }
            set { _message = value; }
        }

        private string _message;

        public ServiceResult()
            : this(ResultType.Success)
        {
        }


        public ServiceResult(ResultType status)
            : this(status, null, default)
        {
        }


        public ServiceResult(ResultType status, string message)
            : this(status, message, default)
        {
        }

        public ServiceResult(ResultType status, T data)
            : this(status, null, data)
        {
        }

        public ServiceResult(ResultType status, string message, T data)
        {
            this.Status = status;
            this.Message = message;
            this.Data = data;
        }
    }

    /// <summary>
    /// Api统一响应结果类
    /// </summary>
    public class ServiceResult : ServiceResult<object>
    {
        /// <summary>
        /// 初始化一个类型的新实例
        /// </summary>
        public ServiceResult()
            : this(ResultType.Info)
        {
        }

        /// <summary>
        /// 初始化一个类型的新实例
        /// </summary>
        public ServiceResult(ResultType status)
            : this(status, null, null)
        {
        }

        /// <summary>
        /// 初始化一个类型的新实例
        /// </summary>
        public ServiceResult(ResultType status, object data)
            : this(status, null, data)
        {
        }

        /// <summary>
        /// 初始化一个类型的新实例
        /// </summary>
        public ServiceResult(ResultType status, string message)
            : this(status, message, null)
        {
        }

        /// <summary>
        /// 初始化一个类型的新实例
        /// </summary>
        public ServiceResult(ResultType status, string message, object data)
            : base(status, message, data)
        {
        }

        /// <summary>
        /// 获取 成功的操作结果
        /// </summary>

        public static ServiceResult Success => new ServiceResult(ResultType.Success);

        /// <summary>
        /// 获取 未变更的操作结果
        /// </summary>

        public static ServiceResult NoChanged => new ServiceResult(ResultType.Info);

        /// <summary>
        /// 获取 出错的操作结果
        /// </summary>

        public static ServiceResult Error => new ServiceResult(ResultType.Error);

        /// <summary>
        /// 获取 登录过期的错误
        /// </summary>

        public static ServiceResult LoginError => new ServiceResult(ResultType.UnAuth);

        /// <summary>
        /// 创建并返回一个成功结果的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> SuccessResult<T>(string message, T data)
        {
            return new ServiceResult<T>(ResultType.Success, message, data);
        }

        /// <summary>
        /// 创建并返回一个成功结果的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> SuccessResult<T>(T data)
        {
            return new ServiceResult<T>(ResultType.Success, data);
        }

        /// <summary>
        /// 创建并返回一个成功结果的实例
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult SuccessResult(string message, object data)
        {
            return new ServiceResult(ResultType.Success, message, data);
        }

        /// <summary>
        /// 创建并返回一个成功结果的实例
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult SuccessResult(object data)
        {
            return new ServiceResult(ResultType.Success, data);
        }


        /// <summary>
        /// 创建并返回一个成功结果的实例
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult SuccessObject(string message, object data)
        {
            return new ServiceResult(ResultType.Success, message, data);
        }

        /// <summary>
        /// 创建并返回一个成功结果的实例
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult SuccessObject(object data)
        {
            return new ServiceResult(ResultType.Success, data);
        }
    }

}
