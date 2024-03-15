// Copyright 2024 The seacraft Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http:www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

package db

type KeyValuePair struct {
	Key   any
	Value any
}

func NewPair(Key any, value any) *KeyValuePair {
	return &KeyValuePair{
		Key:   Key,
		Value: value,
	}
}

type Expression struct {
	List []*KeyValuePair
}

func NewExpression() *Expression {
	return &Expression{
		List: []*KeyValuePair{},
	}
}

func (e *Expression) And(query any, args ...any) {
	e.List = append(e.List, NewPair(query, args))
}
